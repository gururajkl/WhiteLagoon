namespace WhiteLagoon.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext context;
    private readonly DbSet<T> dbSet;

    public Repository(ApplicationDbContext context)
    {
        this.context = context;
        dbSet = context.Set<T>();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        dbSet.Remove(entity);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> entity = dbSet.AsQueryable();

        if (filter is not null)
        {
            entity = entity.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                entity = entity.Include(includeProperty);
            }
        }

        return entity.ToList();
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> entity = dbSet.AsQueryable();
        entity = entity.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                entity = entity.Include(includeProperty);
            }
        }

        return entity.FirstOrDefault()!;
    }

    public bool Any(Expression<Func<T, bool>> filter)
    {
        return dbSet.Any(filter);
    }
}
