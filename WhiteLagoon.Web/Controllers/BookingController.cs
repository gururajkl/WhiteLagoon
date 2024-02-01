namespace WhiteLagoon.Web.Controllers;

public class BookingController : Controller
{
    private readonly IUnitOfWork unitOfWork;

    public BookingController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public IActionResult FinalizeBooking(int villaId, int night, DateTime checkInDate)
    {
        Booking booking = new()
        {
            VillaId = villaId,
            Villa = unitOfWork.Villa.Get(u => u.Id == villaId, includeProperties: "VillaAmenity"),
            CheckInDate = checkInDate,
            CheckOutDate = checkInDate.AddDays(night),
            Nights = night
        };
        booking.TotalCost = booking.Villa.Price * night;

        return View(booking);
    }
}
