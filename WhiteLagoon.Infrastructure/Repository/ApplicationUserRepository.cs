namespace WhiteLagoon.Infrastructure.Repository;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private readonly ApplicationDbContext context;

    public ApplicationUserRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }
}
