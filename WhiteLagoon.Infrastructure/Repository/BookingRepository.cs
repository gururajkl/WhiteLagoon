using WhiteLagoon.Application.Common.Utility;

namespace WhiteLagoon.Infrastructure.Repository;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    private readonly ApplicationDbContext context;

    public BookingRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }

    public void Update(Booking booking)
    {
        context.Bookings.Update(booking);
    }

    public void UpdateStatus(int bookingId, string bookingStatus)
    {
        Booking bookingFromDb = context.Bookings.FirstOrDefault(b => b.Id == bookingId)!;

        if (bookingFromDb is not null)
        {
            bookingFromDb.Status = bookingStatus;

            if (bookingStatus == StaticDetails.StatusCheckIn)
            {
                bookingFromDb.ActualCheckInDate = DateTime.Now;
            }

            if (bookingStatus == StaticDetails.StatusCompleted)
            {
                bookingFromDb.ActualCheckOutDate = DateTime.Now;
            }
        }
    }

    public void UpdateStripePaymentId(int bookingId, string sessionId, string paymentIntentId)
    {
        Booking bookingFromDb = context.Bookings.FirstOrDefault(b => b.Id == bookingId)!;

        if (bookingFromDb is not null)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                bookingFromDb.StripeSessionId = sessionId;
            }

            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                bookingFromDb.StripePaymentIntentId = paymentIntentId;
                bookingFromDb.PaymentDate = DateTime.Now;
            }
        }
    }
}
