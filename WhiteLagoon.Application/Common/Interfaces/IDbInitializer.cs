namespace WhiteLagoon.Application.Common.Interfaces;

/// <summary>
/// DBInitializer that will check if any migrations are pending and apply if it is pending.
/// </summary>
public interface IDbInitializer
{
    /// <summary>
    /// Apply the pending migrations if exists.
    /// </summary>
    void Initialize();
}
