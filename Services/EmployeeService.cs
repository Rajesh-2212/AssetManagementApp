using AssetManagementApp.Models;
using AssetManagementApp.Repositories;

namespace AssetManagementApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IGenericRepository<Employee> _repo;
        public EmployeeService(IGenericRepository<Employee> repo) { _repo = repo; }

        public async Task AddAsync(Employee e) { await _repo.AddAsync(e); await _repo.SaveChangesAsync(); }

        public async Task DeleteAsync(int id) { await _repo.DeleteAsync(id); await _repo.SaveChangesAsync(); }

        public async Task<IEnumerable<Employee>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Employee?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task UpdateAsync(Employee e) { await _repo.UpdateAsync(e); await _repo.SaveChangesAsync(); }
    }
}
