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

    /// <summary>
    /// Villa Number repository which has all CRUD operations.
    /// </summary>
    public IVillaNumberRepository VillaNumber { get; }

    /// <summary>
    /// Amenity repository which has all CRUD operations.
    /// </summary>
    public IAmenityRepository Amenity { get; }

    /// <summary>
    /// Booking repository which has all CRUD operations.
    /// </summary>
    public IBookingRepository Booking { get; }

    /// <summary>
    /// ApplicationUser repository which has all CRUD operations.
    /// </summary>
    public IApplicationUserRepository ApplicationUser { get; }

    /// <summary>
    /// Saves the changes to the database.
    /// </summary>
    public void Save();
}
