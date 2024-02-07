namespace WhiteLagoon.Web.Controllers;

/// <summary>
/// Controller used to manage booking.
/// </summary>
public class BookingController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<ApplicationUser> userManager;

    public BookingController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
    }

    [Authorize]
    public IActionResult Index(string? status = null)
    {
        if (status == null)
        {
            IEnumerable<Booking> bookings = unitOfWork.Booking.GetAll(includeProperties: "Villa,User");
            return View(bookings);
        }
        else
        {
            IEnumerable<Booking> bookingsBasedOnStatus = unitOfWork.Booking.GetAll(b => b.Status == status, includeProperties: "Villa,User");
            return View(bookingsBasedOnStatus);
        }
    }

    [Authorize]
    public IActionResult BookingDetails(int bookingId)
    {
        Booking bookingFromDb = unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperties: "User,Villa");
        return View(bookingFromDb);
    }

    [Authorize]
    public IActionResult FinalizeBooking(int villaId, int night, DateTime checkInDate)
    {
        // Using claims to get the user details.
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        // Using claims to get the user id by getting the first claim.
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        ApplicationUser user = unitOfWork.ApplicationUser.Get(u => u.Id == userId);

        Booking booking = new()
        {
            VillaId = villaId,
            Villa = unitOfWork.Villa.Get(u => u.Id == villaId, includeProperties: "VillaAmenity"),
            CheckInDate = checkInDate,
            CheckOutDate = checkInDate.AddDays(night),
            Nights = night,
            UserId = userId,
            Phone = user.PhoneNumber,
            Email = user.Email,
            Name = user.Name
        };
        booking.TotalCost = booking.Villa.Price * night;

        return View(booking);
    }

    [Authorize]
    [HttpPost]
    public IActionResult FinalizeBooking(Booking booking)
    {
        var villa = unitOfWork.Villa.Get(u => u.Id == booking.VillaId);
        booking.TotalCost = villa.Price * booking.Nights;
        booking.Status = StaticDetails.StatusPending;
        booking.BookingDate = DateTime.Now;

        unitOfWork.Booking.Add(booking);
        unitOfWork.Save();

        var domain = Request.Scheme + "://" + Request.Host.Value + "/";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
            CustomerEmail = booking.Email,
            BillingAddressCollection = "required",
            SuccessUrl = domain + $"booking/BookingConfirmation?bookingId={booking.Id}",
            CancelUrl = domain + $"booking/FinalizeBooking?villaId={booking.VillaId}&night={booking.Nights}&checkInDate={booking.CheckInDate}",
        };

        // Add lineItems for the option.
        options.LineItems.Add(new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(booking.TotalCost * 100),
                Currency = "inr",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = booking.Villa!.Name,
                    Description = booking.Villa!.Description,
                }
            },
            Quantity = 1
        });

        var service = new SessionService();
        Session session = service.Create(options);

        unitOfWork.Booking.UpdateStripePaymentId(booking.Id, session.Id, session.PaymentIntentId);
        unitOfWork.Save();

        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }

    [Authorize]
    public IActionResult BookingConfirmation(int bookingId)
    {
        // Get the booking info from DB.
        Booking bookingFromDb = unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperties: "Villa,User");

        // If the status is pending make it to Approved if payment is success.
        if (bookingFromDb.Status == StaticDetails.StatusPending)
        {
            // This is pending order, confirm if paid or not.
            var services = new SessionService();
            Session session = services.Get(bookingFromDb.StripeSessionId);

            if (session.PaymentStatus == "paid")
            {
                // Update status and paymentIntentId.
                unitOfWork.Booking.UpdateStatus(bookingId, StaticDetails.StatusApproved);
                unitOfWork.Booking.UpdateStripePaymentId(bookingFromDb.Id, session.Id, session.PaymentIntentId);
                unitOfWork.Save();
            }
        }

        return View(bookingId);
    }

    #region API Calls

    // Used for the DataTable.
    [HttpGet]
    [Authorize]
    public IActionResult GetAll()
    {
        IEnumerable<Booking> bookings;

        if (User.IsInRole(StaticDetails.RoleAdmin))
        {
            bookings = unitOfWork.Booking.GetAll(includeProperties: "Villa,User");
        }
        else
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            bookings = unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "Villa,User");
        }
        return Json(new { data = bookings });
    }

    #endregion
}
