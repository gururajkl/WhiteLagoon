namespace WhiteLagoon.Web.Controllers;

[Authorize(Roles = StaticDetails.RoleAdmin)]
public class DashboardController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
    readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
    readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

    public DashboardController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GetTotalBookingRadialChartData()
    {
        var totalBookings = unitOfWork.Booking.GetAll(u => u.Status != StaticDetails.StatusPending
        || u.Status != StaticDetails.StatusCancelled);

        var countByCurrentMonth = totalBookings.Count(b => b.BookingDate >= currentMonthStartDate &&
        b.BookingDate <= DateTime.Now);

        var countByPreviousMonth = totalBookings.Count(b => b.BookingDate >= previousMonthStartDate &&
        b.BookingDate <= currentMonthStartDate);

        return Json(GetRadialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth));
    }

    public IActionResult GetRegisteredUserChartData()
    {
        var totalUser = unitOfWork.ApplicationUser.GetAll();

        var countByCurrentMonth = totalUser.Count(b => b.CreatedAt >= currentMonthStartDate &&
        b.CreatedAt <= DateTime.Now);

        var countByPreviousMonth = totalUser.Count(b => b.CreatedAt >= previousMonthStartDate &&
        b.CreatedAt <= currentMonthStartDate);

        return Json(GetRadialChartDataModel(totalUser.Count(), countByCurrentMonth, countByPreviousMonth));
    }

    public IActionResult GetRevenueChartData()
    {
        var totalBookings = unitOfWork.Booking.GetAll(u => u.Status != StaticDetails.StatusPending
        || u.Status != StaticDetails.StatusCancelled);

        var totalRevenue = Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));

        var countByCurrentMonth = totalBookings.Count(b => b.BookingDate >= currentMonthStartDate &&
        b.BookingDate <= DateTime.Now);

        var countByPreviousMonth = totalBookings.Count(b => b.BookingDate >= previousMonthStartDate &&
        b.BookingDate <= currentMonthStartDate);

        return Json(GetRadialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth));
    }

    public IActionResult GetBookingPieChartData()
    {
        var totalBookings = unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&
        (u.Status != StaticDetails.StatusPending || u.Status != StaticDetails.StatusCancelled));

        var customerWithOneBooking = totalBookings.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

        int bookingByNewCustomer = customerWithOneBooking.Count();
        int bookingByReturningCustomer = totalBookings.Count() - bookingByNewCustomer;

        PieChartViewModel pieChartViewModel = new()
        {
            Labels = new string[] { "New Customer Booking", "Returning Customer Booking" },
            Series = new decimal[] { bookingByNewCustomer, bookingByReturningCustomer }
        };

        return Json(pieChartViewModel);
    }

    public IActionResult GetMemberAndBookingLineChartData()
    {
        var bookingData = unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30)
        && u.BookingDate.Date <= DateTime.Now).GroupBy(b => b.BookingDate.Date).Select(u => new
        {
            DateTime = u.Key,
            NewBookingCount = u.Count()
        });

        var customerData = unitOfWork.ApplicationUser.GetAll(u => u.CreatedAt >= DateTime.Now.AddDays(-30)
        && u.CreatedAt.Date <= DateTime.Now).GroupBy(b => b.CreatedAt.Date).Select(u => new
        {
            DateTime = u.Key,
            NewCustomerCount = u.Count()
        });

        var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime, (booking, customer) => new
        {
            booking.DateTime,
            booking.NewBookingCount,
            NewCustomerCount = customer.Select(x => x.NewCustomerCount).FirstOrDefault()
        });

        var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime, (customer, booking) => new
        {
            customer.DateTime,
            NewBookingCount = booking.Select(x => x.NewBookingCount).FirstOrDefault(),
            customer.NewCustomerCount
        });

        var mergedData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

        var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
        var newCustomerData = mergedData.Select(x => x.NewCustomerCount).ToArray();
        var categories = mergedData.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();

        List<ChartData> chartDataList = new()
        {
            new ChartData
            {
                Name = "New Bookings",
                Data = newBookingData
            },
            new ChartData
            {
                Name = "New Members",
                Data = newCustomerData
            },
        };

        LineChartViewModel chartViewModel = new()
        {
            Categories = categories,
            Series = chartDataList
        };

        return Json(chartViewModel);
    }

    private static RadialBarChartViewModel GetRadialChartDataModel(int totalCount, double currentMonthCount, double prevMonthCount)
    {
        RadialBarChartViewModel radialBarChartViewModel = new();
        int increaseDecreaseRatio = 100;

        if (prevMonthCount != 0)
        {
            increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);
        }

        radialBarChartViewModel.TotalCount = totalCount;
        radialBarChartViewModel.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
        radialBarChartViewModel.HasRatioIncreased = currentMonthCount > prevMonthCount;
        radialBarChartViewModel.Series = new int[] { increaseDecreaseRatio };

        return radialBarChartViewModel;
    }
}
