using AssetManagementApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementApp.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetAssignment> AssetAssignments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
            builder.Entity<Asset>().HasIndex(a => a.SerialNumber).IsUnique(false);

            builder.Entity<AssetAssignment>()
                   .HasOne(a => a.Asset)
                   .WithMany(a => a.Assignments)
                   .HasForeignKey(a => a.AssetId);

            builder.Entity<AssetAssignment>()
                   .HasOne(a => a.Employee)
                   .WithMany()
                   .HasForeignKey(a => a.EmployeeId);
        }
    }
}
