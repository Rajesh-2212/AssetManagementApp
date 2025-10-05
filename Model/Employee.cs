namespace AssetManagementApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public bool Status { get; set; } // Active = true
    }
}
