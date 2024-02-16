namespace WhiteLagoon.Infrastructure;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext applicationDbContext;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<ApplicationUser> userManager;

    public DbInitializer(ApplicationDbContext applicationDbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        this.applicationDbContext = applicationDbContext;
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    public async void Initialize()
    {
        try
        {
            if (applicationDbContext.Database.GetPendingMigrations().Count() > 0)
            {
                applicationDbContext.Database.Migrate();
            }

            // Create roles if no roles exists.
            if (!roleManager.RoleExistsAsync(StaticDetails.RoleAdmin).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleAdmin)).Wait();
                roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleCustomer)).Wait();

                // Create admin user.
                userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "gururajragavendra7@gmail.com",
                    Email = "gururajragavendra7@gmail.com",
                    Name = "Gururaj KL",
                    PhoneNumber = "8618098688",
                    NormalizedEmail = "GURURAJRAGAVENDRA7@GAMIL.COM",
                    NormalizedUserName = "GURURAJ KL"
                }, "Admin!123").GetAwaiter().GetResult();

                ApplicationUser applicationUser = applicationDbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "gururajragavendra7@gmail.com")!;
                userManager.AddToRoleAsync(applicationUser, StaticDetails.RoleAdmin).GetAwaiter().GetResult();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
