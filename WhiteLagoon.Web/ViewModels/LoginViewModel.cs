﻿namespace WhiteLagoon.Web.ViewModels;

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
