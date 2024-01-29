namespace WhiteLagoon.Web.ViewModels;

public class VillaNumberViewModel
{
    public VillaNumber? VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? VillaList { get; set; }
}
