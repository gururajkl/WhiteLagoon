namespace WhiteLagoon.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        this.unitOfWork = unitOfWork;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }

    public IActionResult Login(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        LoginViewModel loginViewModel = new()
        {
            RedirectUrl = returnUrl,
        };

        return View(loginViewModel);
    }
    
    public IActionResult Register()
    {
        return View();
    }
}
