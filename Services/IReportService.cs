namespace AssetManagementApp.Services
{
    public interface IReportService
    {
        Task<IEnumerable<dynamic>> GetAssetsByTypeAsync(string? type);
        Task<IEnumerable<dynamic>> GetAssetsNearingWarrantyExpiryAsync(int days);
        Task<IEnumerable<dynamic>> GetAssignmentHistoryForAssetAsync(int assetId);
    }
}
