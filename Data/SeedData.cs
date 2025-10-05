using AssetManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AssetManagementApp.Data
{
    public static class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var context = services.GetRequiredService<AppDbContext>();

            // Create DB if not exists
            await context.Database.EnsureCreatedAsync();

            // Create admin user
            var adminEmail = "admin@local";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin@123");
            }

            // Seed employees
            if (!await context.Employees.AnyAsync())
            {
                context.Employees.AddRange(
                    new Employee { FullName = "Rajesh H.", Department = "IT", Email = "rajesh@example.com", PhoneNumber = "+911234567890", Designation = "Developer", Status = true },
                    new Employee { FullName = "Sita K.", Department = "Operations", Email = "sita@example.com", PhoneNumber = "+911234567891", Designation = "Manager", Status = true }
                );
            }

            // Seed assets
            if (!await context.Assets.AnyAsync())
            {
                context.Assets.AddRange(
                    new Asset
                    {
                        Name = "Dell Laptop",
                        AssetType = "Laptop",
                        MakeModel = "Dell XPS 13",
                        SerialNumber = "DLXPS001",
                        PurchaseDate = DateTime.Parse("2023-03-01"),
                        WarrantyExpiryDate = DateTime.Parse("2025-03-01"),
                        Condition = AssetCondition.Good,
                        Status = AssetStatus.Available,
                        IsSpare = false,
                        Specifications = "i7,16GB,512GB"
                    },
                    new Asset
                    {
                        Name = "Logitech Mouse",
                        AssetType = "Peripheral",
                        MakeModel = "MX Master 3",
                        SerialNumber = "MOUSE001",
                        PurchaseDate = DateTime.Now,
                        WarrantyExpiryDate = DateTime.Now.AddYears(1),
                        Condition = AssetCondition.New,
                        Status = AssetStatus.Available,
                        IsSpare = true,
                        Specifications = "Wireless"
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
