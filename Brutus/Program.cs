using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Brutus.Data;
using Brutus.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BrutusContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BrutusContext") ?? throw new InvalidOperationException("Connection string 'BrutusContext' not found.")));

// Dodaj Identity, wykorzystując ten sam kontekst bazy danych
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<BrutusContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Inicjalizacja ról
await InitializeRoles(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

// Metoda do inicjalizacji ról
async Task InitializeRoles(IHost app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = { "Admin", "Rodzic", "Uczen", "Nauczyciel" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Utwórz role i zapisz w bazie danych
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}