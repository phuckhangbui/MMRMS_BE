using Microsoft.EntityFrameworkCore;

namespace DAO
{

    public class BaseDao<T> where T : class
    {
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> list;
            try
            {
                var _context = new MmrmsContext();
                DbSet<T> _dbSet = _context.Set<T>();
                list = await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                var _context = new MmrmsContext();
                DbSet<T> _dbSet = _context.Set<T>();
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var _context = new MmrmsContext();
                DbSet<T> _dbSet = _context.Set<T>();
                var tracker = _context.Attach(entity);
                tracker.State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveAsync(T entity)
        {
            try
            {
                var _context = new MmrmsContext();
                DbSet<T> _dbSet = _context.Set<T>();
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
