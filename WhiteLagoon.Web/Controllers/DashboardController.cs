using WhiteLagoon.Application.Services.Interface;

namespace WhiteLagoon.Web.Controllers;

[Authorize(Roles = StaticDetails.RoleAdmin)]
public class DashboardController : Controller
{
    private readonly IDashboardService dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GetTotalBookingRadialChartData()
    {
        return Json(dashboardService.GetTotalBookingRadialChartData());
    }

    public IActionResult GetRegisteredUserChartData()
    {
        return Json(dashboardService.GetRegisteredUserChartData());
    }

    public IActionResult GetRevenueChartData()
    {
        return Json(dashboardService.GetRevenueChartData());
    }

    public IActionResult GetBookingPieChartData()
    {

        return Json(dashboardService.GetBookingPieChartData());
    }

    public IActionResult GetMemberAndBookingLineChartData()
    {
        return Json(dashboardService.GetMemberAndBookingLineChartData());
    }
}
