namespace WhiteLagoon.Web.ViewModels;

// ViewModel used for the register view.
public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password does not match")]
    [Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Your name is important for us...")]
    public string? Name { get; set; }
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }
    public string? RedirectUrl { get; set; }
    /// <summary>
    /// Contains the role of the user.
    /// </summary>
    public string? Role { get; set; }
    /// <summary>
    /// Has roles in the form of Iterator and validates never.
    /// </summary>
    [ValidateNever]
    public IEnumerable<SelectListItem>? RoleList { get; set; }
}
