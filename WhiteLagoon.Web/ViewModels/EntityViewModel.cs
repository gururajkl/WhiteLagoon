namespace WhiteLagoon.Web.ViewModels;

public class EntityViewModel<T> where T : class
{
    public T? Entity { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? VillaList { get; set; }
}
