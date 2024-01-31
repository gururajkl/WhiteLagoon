namespace WhiteLagoon.Web.ViewModels;

/// <summary>
/// ViewModel of generic type.
/// </summary>
/// <typeparam name="T">Generic type</typeparam>
public class EntityViewModel<T> where T : class
{
    /// <summary>
    /// Property of generic type.
    /// </summary>
    public T? Entity { get; set; }
    /// <summary>
    /// IEnumerable of selectList item.
    /// </summary>
    [ValidateNever]
    public IEnumerable<SelectListItem>? VillaList { get; set; }
}
