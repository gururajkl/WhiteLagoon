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
}
