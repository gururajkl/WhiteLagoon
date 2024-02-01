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
            CheckInDate = DateOnly.FromDateTime(DateTime.Now)
        };
        return View(homeViewModel);
    }

    [HttpPost]
    public IActionResult Index(HomeViewModel homeViewModel)
    {
        homeViewModel.VillaList = unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");

        foreach (var villa in homeViewModel.VillaList)
        {
            if (villa.Id % 2 == 0)
            {
                villa.IsAvailable = false;
            }
        }

        return View(homeViewModel);
    }

    /// <summary>
    /// Calling this method in JQuery success function and get the needed fields and to make sure that partial view only refreshes.
    /// </summary>
    /// <param name="nights">Nights from the UI</param>
    /// <param name="checkInDate">CheckInDate from the UI</param>
    /// <returns>Goes to partial view with homeViewModel</returns>
    public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
    {
        // Thread.Sleep(2000); use this to see the Loader for a while.
        IEnumerable<Villa> villaList = unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");

        foreach (var villa in villaList)
        {
            if (villa.Id % 2 == 0)
            {
                villa.IsAvailable = false;
            }
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
