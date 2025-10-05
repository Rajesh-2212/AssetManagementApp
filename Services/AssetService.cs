using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AssetManagementApp.Data;
using AssetManagementApp.Models;

namespace AssetManagementApp.Services
{
    public class AssetService : IAssetService
    {
                private readonly AppDbContext _context;

        public AssetService(AppDbContext context)
        {
            _context = context;
        }

        private DbSet<Asset> Assets => _context.Set<Asset>();
        private DbSet<AssetAssignment> AssetAssignments => _context.Set<AssetAssignment>();
        private DbSet<Employee> Employees => _context.Set<Employee>();

        public async Task<List<Asset>> GetAllAsync()
        {
            return await Assets
                .Include(a => a.Assignments)
                .ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await Assets
                .Include(a => a.Assignments)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Asset asset)
        {
            Assets.Add(asset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Asset asset)
        {
            Assets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var a = await Assets.FindAsync(id);
            if (a == null) return;
            Assets.Remove(a);
            await _context.SaveChangesAsync();
        }

        // --- Assignment-related helpers ---

        public async Task<List<Asset>> GetAvailableAssetsAsync()
        {
            return await Assets
                .Include(a => a.Assignments)
                .Where(a => a.Status == AssetStatus.Available)
                .ToListAsync();
        }

        public async Task<List<AssetAssignment>> GetCurrentAssignmentsAsync()
        {
            return await AssetAssignments
                .Include(x => x.Asset)
                .Include(x => x.Employee)
                .Where(x => x.ReturnedDate == null)
                .OrderByDescending(x => x.AssignedDate)
                .ToListAsync();
        }

        public async Task<List<AssetAssignment>> GetAssignmentHistoryByEmployeeAsync(int employeeId)
        {
            return await AssetAssignments
                .Include(x => x.Asset)
                .Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.AssignedDate)
                .ToListAsync();
        }

        public async Task AssignToEmployeeAsync(int assetId, int employeeId, string? notes)
        {
            // transactional: create assignment + set asset status to Assigned
            using IDbContextTransaction tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var asset = await Assets.FindAsync(assetId);
                if (asset == null) throw new InvalidOperationException($"Asset {assetId} not found.");
                if (asset.Status != AssetStatus.Available)
                    throw new InvalidOperationException("Asset is not available for assignment.");

                var assignment = new AssetAssignment
                {
                    AssetId = assetId,
                    EmployeeId = employeeId,
                    AssignedDate = DateTime.UtcNow,
                    Notes = notes ?? string.Empty
                };

                AssetAssignments.Add(assignment);

                asset.Status = AssetStatus.Assigned;
                Assets.Update(asset);

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task ReturnAssignmentAsync(int assignmentId)
        {
            using IDbContextTransaction tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var assignment = await AssetAssignments
                    .Include(a => a.Asset)
                    .FirstOrDefaultAsync(a => a.Id == assignmentId);

                if (assignment == null) throw new InvalidOperationException($"Assignment {assignmentId} not found.");
                if (assignment.ReturnedDate != null) return; // already returned

                assignment.ReturnedDate = DateTime.UtcNow;
                AssetAssignments.Update(assignment);

                if (assignment.Asset != null)
                {
                    assignment.Asset.Status = AssetStatus.Available;
                    Assets.Update(assignment.Asset);
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<AssetAssignment>> GetAllAssignmentsAsync()
        {
            return await AssetAssignments
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .OrderByDescending(a => a.AssignedDate)
                .ToListAsync();
        }
    }
}
