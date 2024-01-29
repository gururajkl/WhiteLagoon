namespace WhiteLagoon.Infrastructure;

public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
{
    private readonly ApplicationDbContext context;

    public VillaNumberRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }

    public void Update(VillaNumber villaNumber)
    {
        context.VillaNumbers.Update(villaNumber);
    }
}
