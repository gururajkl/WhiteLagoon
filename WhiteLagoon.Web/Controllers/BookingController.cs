using System.Security.Claims;

namespace WhiteLagoon.Web.Controllers;

public class BookingController : Controller
{
    private readonly IUnitOfWork unitOfWork;

    public BookingController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
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

        return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
    }

    [Authorize]
    public IActionResult BookingConfirmation(int bookingId)
    {
        return View(bookingId);
    }
}
