namespace WhiteLagoon.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    public IVillaRepository Villa { get; private set; }
    public IVillaNumberRepository VillaNumber { get; private set; }

    public IAmenityRepository Amenity { get; private set; }

    private readonly ApplicationDbContext context;

    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context;
        Villa = new VillaRepository(context);
        VillaNumber = new VillaNumberRepository(context);
        Amenity = new AmenityRepository(context);
    }

    public void Save()
    {
        context.SaveChanges();
    }
}
