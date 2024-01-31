namespace WhiteLagoon.Web.ViewModels;

/// <summary>
/// Viewmodel for the home with some properties.
/// </summary>
public class HomeViewModel
{
    public IEnumerable<Villa>? VillaList { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly? CheckOutDate { get; set; }
    public int Nights { get; set; }
}
