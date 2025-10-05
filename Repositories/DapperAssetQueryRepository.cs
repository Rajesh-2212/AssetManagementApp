using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AssetManagementApp.Repositories
{
    public class DapperAssetQueryRepository : IAssetQueryRepository
    {
        private readonly IConfiguration _config;
        public DapperAssetQueryRepository(IConfiguration config) { _config = config; }

        private IDbConnection Connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<dynamic>> GetAssetsByTypeAsync(string? assetType)
        {
            using var conn = Connection;
            var sql = "SELECT * FROM Assets WHERE (@type IS NULL OR AssetType = @type)";
            return await conn.QueryAsync(sql, new { type = assetType });
        }

        public async Task<IEnumerable<dynamic>> GetAssetsNearingWarrantyExpiryAsync(int days)
        {
            using var conn = Connection;
            var sql = "SELECT * FROM Assets WHERE WarrantyExpiryDate IS NOT NULL AND DATEDIFF(day, GETDATE(), WarrantyExpiryDate) <= @days";
            return await conn.QueryAsync(sql, new { days });
        }

        public async Task<IEnumerable<dynamic>> GetAssignmentHistoryForAssetAsync(int assetId)
        {
            using var conn = Connection;
            var sql = @"
                SELECT aa.Id, aa.AssetId, aa.EmployeeId, aa.AssignedDate, aa.ReturnedDate, aa.Notes, e.FullName
                FROM AssetAssignments aa
                JOIN Employees e ON aa.EmployeeId = e.Id
                WHERE aa.AssetId = @assetId
                ORDER BY aa.AssignedDate DESC";
            return await conn.QueryAsync(sql, new { assetId });
        }
    }
}
