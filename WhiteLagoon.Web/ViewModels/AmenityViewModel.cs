namespace WhiteLagoon.Web.ViewModels;

public class AmenityViewModel
{
    public Amenity? Amenity { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? VillaList { get; set; }
}
