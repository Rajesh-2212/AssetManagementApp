using AssetManagementApp.Models;

namespace AssetManagementApp.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee e);
        Task UpdateAsync(Employee e);
        Task DeleteAsync(int id);
    }
}
