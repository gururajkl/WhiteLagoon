namespace WhiteLagoon.Application.Services.Interface;

public interface IDashboardService
{
    RadialBarChartDTO GetTotalBookingRadialChartData();
    RadialBarChartDTO GetRegisteredUserChartData();
    RadialBarChartDTO GetRevenueChartData();
    PieChartDTO GetBookingPieChartData();
    LineChartDTO GetMemberAndBookingLineChartData();
}
