using System.Linq.Expressions;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces;

/// <summary>
/// Encapsulates the VillaRepo.
/// </summary>
public interface IVillaRepository
{
    /// <summary>
    /// Gives the villa lists.
    /// </summary>
    /// <param name="filter">Lambda expression.</param>
    /// <param name="includeProperties">The properties need to include. (Props should be case sensitive)</param>
    /// <returns></returns>
    public IEnumerable<Villa> GetAllVillas(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null);
    /// <summary>
    /// Gives the villa.
    /// </summary>
    /// <param name="filter">Lambda expression.</param>
    /// <param name="includeProperties">The properties need to include.</param>
    /// <returns></returns>
    public Villa GetVilla(Expression<Func<Villa, bool>> filter, string? includeProperties = null);
    /// <summary>
    /// Creates new villa.
    /// </summary>
    /// <param name="villa">Villa entity.</param>
    public void AddVilla(Villa villa);
    /// <summary>
    /// Updates the existing villa.
    /// </summary>
    /// <param name="villa">Villa entity.</param>
    public void UpdateVilla(Villa villa);
    /// <summary>
    /// Deletes the exisiting villa.
    /// </summary>
    /// <param name="villa">Villa entity.</param>
    public void DeleteVilla(Villa villa);
    /// <summary>
    /// Saves the changes to the database.
    /// </summary>
    public void Save();
}
