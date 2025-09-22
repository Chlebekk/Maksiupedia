using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Maksiupedia.Data;
using Maksiupedia.Models;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=Maksiupedia.db"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var config = services.GetRequiredService<IConfiguration>();

    string[] roles = new[] { "Manager", "Admin", "Owner" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var r = new IdentityRole(role);
            await roleManager.CreateAsync(r);
        }
    }

    var ownerEmail = config["Seed:OwnerEmail"] ?? Environment.GetEnvironmentVariable("MAKSI_OWNER_EMAIL") ?? "owner@example.com";
    var ownerPassword = config["Seed:OwnerPassword"] ?? Environment.GetEnvironmentVariable("MAKSI_OWNER_PW") ?? "OwnerPassword123!";
    var ownerDisplay = config["Seed:OwnerDisplayName"] ?? "Owner";

    var existing = await userManager.FindByEmailAsync(ownerEmail);
    if (existing == null)
    {
        var owner = new ApplicationUser
        {
            UserName = ownerEmail,
            Email = ownerEmail,
            DisplayName = ownerDisplay,
            EmailConfirmed = true
        };
        var createResult = await userManager.CreateAsync(owner, ownerPassword);
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(owner, "Owner");
        }
        else
        {
            var combined = string.Join("; ", createResult.Errors.Select(e => e.Description));
            throw new Exception($"Failed to create owner user: {combined}");
        }
    }
    else
    {
        var hasOwnerRole = (await userManager.GetRolesAsync(existing)).Contains("Owner");
        if (!hasOwnerRole)
        {
            await userManager.AddToRoleAsync(existing, "Owner");
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
