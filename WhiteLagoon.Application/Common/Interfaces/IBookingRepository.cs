namespace WhiteLagoon.Application.Common.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    public void Update(Booking booking);
    public void UpdateStatus(int bookingId, string bookingStatus);
    public void UpdateStripePaymentId(int bookingId, string sessionId, string paymentIntentId);
}
