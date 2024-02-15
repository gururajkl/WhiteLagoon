namespace WhiteLagoon.Web.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        HomeViewModel homeViewModel = new()
        {
            VillaList = unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity"),
            Nights = 1,
            CheckInDate = DateTime.Now
        };

        if (!User.IsInRole(StaticDetails.RoleAdmin))
        {
            return View(homeViewModel);
        }
        else
        {
            return RedirectToAction(nameof(AdminIndex));
        }
    }

    [Authorize(Roles = StaticDetails.RoleAdmin)]
    public IActionResult AdminIndex()
    {
        HomeViewModel homeViewModel = new()
        {
            VillaList = unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity"),
            Nights = 1,
            CheckInDate = DateTime.Now
        };
        return View(homeViewModel);
    }

    /// <summary>
    /// Calling this method in JQuery success function and get the needed fields and to make sure that partial view only refreshes.
    /// </summary>
    /// <param name="nights">Nights from the UI</param>
    /// <param name="checkInDate">CheckInDate from the UI</param>
    /// <returns>Goes to partial view with homeViewModel</returns>
    [HttpPost]
    public IActionResult GetVillasByDate(int nights, DateTime checkInDate)
    {
        Thread.Sleep(1200); // use this to see the Loader for a while.

        List<Villa> villaList = unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
        List<Booking> bookings = unitOfWork.Booking.GetAll(u => u.Status == StaticDetails.StatusApproved || u.Status == StaticDetails.StatusCheckIn).ToList();
        List<VillaNumber> villaNumbers = unitOfWork.VillaNumber.GetAll().ToList();

        foreach (var villa in villaList)
        {
            int roomAvailable = StaticDetails.VillaRoomsAvailableCount(villa.Id, villaNumbers, checkInDate, nights, bookings);
            villa.IsAvailable = roomAvailable > 0 ? true : false;
        }

        HomeViewModel homeViewModel = new()
        {
            CheckInDate = checkInDate,
            VillaList = villaList,
            Nights = nights
        };

        return PartialView("_VillaList", homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult NotFoundPage()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
