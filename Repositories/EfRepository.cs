using AssetManagementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementApp.Repositories
{
    public class EfRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null) _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task UpdateAsync(T entity) { _dbSet.Update(entity); await Task.CompletedTask; }
    }
}
