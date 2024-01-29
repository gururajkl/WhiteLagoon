using WhiteLagoon.Application.Common.Interfaces;

namespace WhiteLagoon.Application;

/// <summary>
/// Interface which encapsulates all the Interfaces.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Villa repository which has all CRUD operations.
    /// </summary>
    public IVillaRepository Villa { get; }
    public IVillaNumberRepository VillaNumber { get; }
}
