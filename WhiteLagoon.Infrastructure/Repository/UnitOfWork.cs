using WhiteLagoon.Application;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    public IVillaRepository Villa { get; private set; }
    public IVillaNumberRepository VillaNumber { get; private set; }
    private readonly ApplicationDbContext context;

    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context;
        Villa = new VillaRepository(context);
        VillaNumber = new VillaNumberRepository(context);
    }
}
