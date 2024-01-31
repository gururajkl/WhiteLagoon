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

    public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
    {
        Thread.Sleep(2000);
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
