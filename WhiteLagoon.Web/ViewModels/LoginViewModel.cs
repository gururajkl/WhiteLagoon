namespace WhiteLagoon.Web.ViewModels;

/// <summary>
/// Viewmodel for the login with some properties.
/// </summary>
public class LoginViewModel
{
    [Required]
    public string? Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    [Display(Name = "Remember Me?")]
    public bool RememberMe { get; set; }
    public string? RedirectUrl { get; set; }
}
