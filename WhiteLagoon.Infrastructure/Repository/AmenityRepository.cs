namespace WhiteLagoon.Infrastructure.Repository;

public class AmenityRepository : Repository<Amenity>, IAmenityRepository
{
    private readonly ApplicationDbContext context;

    public AmenityRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }

    public void Update(Amenity amenity)
    {
        context.Amenities.Update(amenity);
    }
}
