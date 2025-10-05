namespace AssetManagementApp.Repositories
{
    public interface IAssetQueryRepository
    {
        Task<IEnumerable<dynamic>> GetAssetsByTypeAsync(string? assetType);
        Task<IEnumerable<dynamic>> GetAssetsNearingWarrantyExpiryAsync(int days);
        Task<IEnumerable<dynamic>> GetAssignmentHistoryForAssetAsync(int assetId);
    }
}
