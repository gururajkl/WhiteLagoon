namespace WhiteLagoon.Application.Services.Implementation;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork unitOfWork;
    static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
    readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
    readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

    public DashboardService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public PieChartDTO GetBookingPieChartData()
    {
        var totalBookings = unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&
        (u.Status != StaticDetails.StatusPending || u.Status != StaticDetails.StatusCancelled));

        var customerWithOneBooking = totalBookings.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

        int bookingByNewCustomer = customerWithOneBooking.Count();
        int bookingByReturningCustomer = totalBookings.Count() - bookingByNewCustomer;

        PieChartDTO pieChartDTO = new()
        {
            Labels = new string[] { "New Customer Booking", "Returning Customer Booking" },
            Series = new decimal[] { bookingByNewCustomer, bookingByReturningCustomer }
        };

        return pieChartDTO;
    }

    public LineChartDTO GetMemberAndBookingLineChartData()
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

        LineChartDTO chartDTO = new()
        {
            Categories = categories,
            Series = chartDataList
        };

        return chartDTO;
    }

    public RadialBarChartDTO GetRegisteredUserChartData()
    {
        var totalUser = unitOfWork.ApplicationUser.GetAll();

        var countByCurrentMonth = totalUser.Count(b => b.CreatedAt >= currentMonthStartDate &&
        b.CreatedAt <= DateTime.Now);

        var countByPreviousMonth = totalUser.Count(b => b.CreatedAt >= previousMonthStartDate &&
        b.CreatedAt <= currentMonthStartDate);

        return StaticDetails.GetRadialChartDataModel(totalUser.Count(), countByCurrentMonth, countByPreviousMonth);
    }

    public RadialBarChartDTO GetRevenueChartData()
    {
        var totalBookings = unitOfWork.Booking.GetAll(u => u.Status != StaticDetails.StatusPending
        || u.Status != StaticDetails.StatusCancelled);

        var totalRevenue = Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));

        var countByCurrentMonth = totalBookings.Count(b => b.BookingDate >= currentMonthStartDate &&
        b.BookingDate <= DateTime.Now);

        var countByPreviousMonth = totalBookings.Count(b => b.BookingDate >= previousMonthStartDate &&
        b.BookingDate <= currentMonthStartDate);

        return StaticDetails.GetRadialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth);
    }

    public RadialBarChartDTO GetTotalBookingRadialChartData()
    {
        var totalBookings = unitOfWork.Booking.GetAll(u => u.Status != StaticDetails.StatusPending
        || u.Status != StaticDetails.StatusCancelled);

        var countByCurrentMonth = totalBookings.Count(b => b.BookingDate >= currentMonthStartDate &&
        b.BookingDate <= DateTime.Now);

        var countByPreviousMonth = totalBookings.Count(b => b.BookingDate >= previousMonthStartDate &&
        b.BookingDate <= currentMonthStartDate);

        return StaticDetails.GetRadialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth);
    }
}
