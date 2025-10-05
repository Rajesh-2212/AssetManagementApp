using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagementApp.Models;

namespace AssetManagementApp.Services
{
    public interface IAssetService
    {
        Task<List<Asset>> GetAllAsync();
        Task<Asset?> GetByIdAsync(int id);
        Task AddAsync(Asset asset);
        Task UpdateAsync(Asset asset);
        Task DeleteAsync(int id);

        // Assignment-related operations
        Task AssignToEmployeeAsync(int assetId, int employeeId, string? notes);
        
        // New: helper methods used by Assignments.razor
        Task<List<Asset>> GetAvailableAssetsAsync();
        Task<List<AssetAssignment>> GetCurrentAssignmentsAsync();
        Task<List<AssetAssignment>> GetAssignmentHistoryByEmployeeAsync(int employeeId);
        Task ReturnAssignmentAsync(int assignmentId);
        Task<IEnumerable<AssetAssignment>> GetAllAssignmentsAsync();
    }
}
