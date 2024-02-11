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
        IEnumerable<Booking> bookings;

        if (string.IsNullOrEmpty(status))
        {
            status = StaticDetails.StatusApproved;
        }

        if (User.IsInRole(StaticDetails.RoleAdmin))
        {
            bookings = unitOfWork.Booking.GetAll(b => b.Status == status, includeProperties: "Villa,User");
            return View(bookings);
        }
        else
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            bookings = unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "Villa,User").Where(u => u.Status == status);
            return View(bookings);
        }
    }

    [Authorize]
    public IActionResult BookingDetails(int bookingId)
    {
        Booking bookingFromDb = unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperties: "User,Villa");

        if (bookingFromDb.VillaNumber == 0 && bookingFromDb.Status == StaticDetails.StatusApproved)
        {
            var availableVillaList = AssignVillaNumberByVilla(bookingFromDb.VillaId);
            bookingFromDb.VillaNumbers = unitOfWork.VillaNumber.GetAll(u => u.VillaId == bookingFromDb.VillaId && availableVillaList.Any(x => x == u.Villa_Number)).ToList();
        }

        return View(bookingFromDb);
    }

    private List<int> AssignVillaNumberByVilla(int villaId)
    {
        // Get villa numbers based on villa.
        var villaNumbers = unitOfWork.VillaNumber.GetAll(u => u.VillaId == villaId).ToList();

        // Get bookings based on villa and checkIn status.
        var checkedInVilla = unitOfWork.Booking.GetAll(u => u.VillaId == villaId && u.Status == StaticDetails.StatusCheckIn).Select(u => u.VillaNumber).ToList();

        List<int> availableVillaNumber = new();

        foreach (var villaNumber in villaNumbers)
        {
            if (!checkedInVilla.Contains(villaNumber.Villa_Number))
            {
                availableVillaNumber.Add(villaNumber.Villa_Number);
            }
        }

        return availableVillaNumber;
    }

    [Authorize(Roles = StaticDetails.RoleAdmin)]
    [HttpPost]
    public IActionResult CheckIn(Booking booking)
    {
        unitOfWork.Booking.UpdateStatus(booking.Id, StaticDetails.StatusCheckIn, booking.VillaNumber);
        unitOfWork.Save();
        TempData["success"] = "Booking updated successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }

    [Authorize(Roles = StaticDetails.RoleAdmin)]
    [HttpPost]
    public IActionResult CheckOut(Booking booking)
    {
        unitOfWork.Booking.UpdateStatus(booking.Id, StaticDetails.StatusCompleted, booking.VillaNumber);
        unitOfWork.Save();
        TempData["success"] = "Booking completed successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }

    [Authorize(Roles = StaticDetails.RoleAdmin)]
    [HttpPost]
    public IActionResult CancelBooking(Booking booking)
    {
        unitOfWork.Booking.UpdateStatus(booking.Id, StaticDetails.StatusCancelled, 0);
        unitOfWork.Save();
        TempData["success"] = "Booking cancelled successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
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

        List<Booking> bookings = unitOfWork.Booking.GetAll(u => u.Status == StaticDetails.StatusApproved || u.Status == StaticDetails.StatusCheckIn).ToList();
        List<VillaNumber> bookedVillaNumbers = unitOfWork.VillaNumber.GetAll().ToList();

        // If no rooms are available.
        int roomAvailable = StaticDetails.VillaRoomsAvailableCount(villa.Id, bookedVillaNumbers, booking.CheckInDate, booking.Nights, bookings);
        if (roomAvailable == 0)
        {
            TempData["error"] = "Room has been sold out";
            return RedirectToAction(nameof(FinalizeBooking), new
            {
                villaId = booking.VillaId,
                night = booking.Nights,
                checkInDate = booking.CheckInDate
            });
        }

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
                unitOfWork.Booking.UpdateStatus(bookingId, StaticDetails.StatusApproved, 0);
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
