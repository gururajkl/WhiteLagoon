using System.Linq.Expressions;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces;

/// <summary>
/// Encapsulates the VillaRepository.
/// </summary>
public interface IVillaRepository : IRepository<Villa>
{
    /// <summary>
    /// Updates the existing villa.
    /// </summary>
    /// <param name="villa">Villa entity.</param>
    public void UpdateVilla(Villa villa);
}
