﻿namespace WhiteLagoon.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gives the generic type lists.
    /// </summary>
    /// <param name="filter">Lambda expression.</param>
    /// <param name="includeProperties">The properties need to include. (Props should be case sensitive)</param>
    /// <returns>Enumerator</returns>
    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

    /// <summary>
    /// Gives the value of generic type.
    /// </summary>
    /// <param name="filter">Lambda expression.</param>
    /// <param name="includeProperties">The properties need to include.</param>
    /// <returns>Enumerator of Generic type.</returns>
    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

    /// <summary>
    /// Creates new generic entity object.
    /// </summary>
    /// <param name="T"><typeparam name="T" /> entity.</param>
    public void Add(T entity);

    /// <summary>
    /// Deletes the exisiting generic entity object.
    /// </summary>
    /// <param name="T"><typeparam name="T" /> entity.</param>
    public void Delete(T entity);

    /// <summary>
    /// Used to check if the value exists of particular entity.
    /// </summary>
    /// <param name="filter">Lambda expression</param>
    public bool Any(Expression<Func<T, bool>> filter);
}
