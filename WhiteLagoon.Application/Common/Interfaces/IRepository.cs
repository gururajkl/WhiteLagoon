using System.Linq.Expressions;

namespace WhiteLagoon.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gives the <typeparam name="T" /> lists.
    /// </summary>
    /// <param name="filter">Lambda expression.</param>
    /// <param name="includeProperties">The properties need to include. (Props should be case sensitive)</param>
    /// <returns>Enumerator</returns>
    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

    /// <summary>
    /// Gives the <typeparam name="T" />.
    /// </summary>
    /// <param name="filter">Lambda expression.</param>
    /// <param name="includeProperties">The properties need to include.</param>
    /// <returns>Enumerator</returns>
    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

    /// <summary>
    /// Creates new <typeparam name="T" />.
    /// </summary>
    /// <param name="T"><typeparam name="T" /> entity.</param>
    public void Add(T entity);

    /// <summary>
    /// Deletes the exisiting T.
    /// </summary>
    /// <param name="T"><typeparam name="T" /> entity.</param>
    public void Delete(T entity);
}
