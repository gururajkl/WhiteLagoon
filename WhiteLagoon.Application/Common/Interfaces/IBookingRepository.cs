namespace WhiteLagoon.Application.Common.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    public void Update(Booking booking);
}
