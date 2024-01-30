namespace WhiteLagoon.Web.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

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
