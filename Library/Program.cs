using Library.Data;
using Library.Infrastructure;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── MVC ──────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ── Repository (application data) ───────────────────────────────────────────
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

// ── App Database (Books + Genres) ────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// ── Identity Database (Users + Roles) ───────────────────────────────────────
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

// ── Identity Configuration ───────────────────────────────────────────────────
builder.Services.AddIdentity<AppUser, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = true;
    opts.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders();

// ── Custom Validators ────────────────────────────────────────────────────────
builder.Services.AddScoped<IPasswordValidator<AppUser>, CustomPasswordValidator>();
builder.Services.AddScoped<IUserValidator<AppUser>, CustomUserValidator>();

// ── Routing options ──────────────────────────────────────────────────────────
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

var app = builder.Build();

// ── Middleware ───────────────────────────────────────────────────────────────
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ── Routes ───────────────────────────────────────────────────────────────────
// Route for sorting + paging
app.MapControllerRoute(
    name: "sortpage",
    pattern: "Book/OrderBy{sortBy}/Page{page}",
    defaults: new { controller = "Book", action = "Index", page = 1 });

// Route for sorting only
app.MapControllerRoute(
    name: "sort",
    pattern: "Book/OrderBy{sortBy}",
    defaults: new { controller = "Book", action = "Index" });

// Route for paging only
app.MapControllerRoute(
    name: "page",
    pattern: "Book/Page{page}",
    defaults: new { controller = "Book", action = "Index", page = 1 });

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ── Seed Data ────────────────────────────────────────────────────────────────
SeedData.EnsurePopulated(app);
SeedIdentityData.EnsurePopulated(app);

app.Run();
