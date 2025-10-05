using AssetManagementApp.Repositories;

namespace AssetManagementApp.Services
{
    public class ReportService : IReportService
    {
        private readonly IAssetQueryRepository _queryRepo;

        public ReportService(IAssetQueryRepository queryRepo) { _queryRepo = queryRepo; }

        public async Task<IEnumerable<dynamic>> GetAssetsByTypeAsync(string? type) => await _queryRepo.GetAssetsByTypeAsync(type);

        public async Task<IEnumerable<dynamic>> GetAssetsNearingWarrantyExpiryAsync(int days) => await _queryRepo.GetAssetsNearingWarrantyExpiryAsync(days);

        public async Task<IEnumerable<dynamic>> GetAssignmentHistoryForAssetAsync(int assetId) => await _queryRepo.GetAssignmentHistoryForAssetAsync(assetId);
    }
}
