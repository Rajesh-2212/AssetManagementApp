using AssetManagementApp.Data;
using AssetManagementApp.Repositories;
using AssetManagementApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>();

// Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Layers
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IAssetQueryRepository, DapperAssetQueryRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

// Seed DB
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await SeedData.EnsureSeedData(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
