namespace WhiteLagoon.Web.ViewModels;

/// <summary>
/// Viewmodel for the home with some properties.
/// </summary>
public class HomeViewModel
{
    public IEnumerable<Villa>? VillaList { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public int Nights { get; set; }
}
