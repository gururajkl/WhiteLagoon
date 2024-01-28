using System.Linq.Expressions;
using WhiteLagoon.Application.Common.Interfaces;

namespace WhiteLagoon.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    public void Add(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        throw new NotImplementedException();
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        throw new NotImplementedException();
    }
}
