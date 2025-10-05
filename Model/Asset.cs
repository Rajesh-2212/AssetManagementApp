using System.ComponentModel.DataAnnotations;

namespace AssetManagementApp.Models
{
    public enum AssetCondition { New, Good, NeedsRepair, Damaged }
    public enum AssetStatus { Available, Assigned, UnderRepair, Retired }

    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssetType { get; set; }
        public string MakeModel { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }
        public AssetCondition Condition { get; set; }
        public AssetStatus Status { get; set; }
        public bool IsSpare { get; set; }
        public string Specifications { get; set; } = string.Empty;

        public ICollection<AssetAssignment> Assignments { get; set; } = new List<AssetAssignment>();
    }
}
