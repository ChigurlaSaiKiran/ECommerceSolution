using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        // ✅ ✅ ✅ NEW: Overload with Include!
        public async Task<T?> GetByIdAsync(
            int id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync(); // Save immediately
        }

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
    //public class Repository<T> : IRepository<T> where T : class
    //{
    //    private readonly AppDbContext _context;
    //    private readonly DbSet<T> _dbSet;

    //    public Repository(AppDbContext context)
    //    {
    //        _context = context;
    //        _dbSet = context.Set<T>();
    //    }

    //    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();


    //    public async Task<IEnumerable<T>> GetAllAsync(
    //        Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
    //    {
    //        IQueryable<T> query = _dbSet;
    //        if (include != null)
    //        {
    //            query = include(query);
    //        }

    //        return await query.ToListAsync();
    //    }

    //    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    //    //public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    //    //    => await _dbSet.Where(predicate).ToListAsync();

    //   // public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    //    public void Update(T entity) => _dbSet.Update(entity);

    //    public void Delete(T entity) => _dbSet.Remove(entity);
    //    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    //    public async Task AddAsync(T entity)
    //    {
    //        await _context.Set<T>().AddAsync(entity);
    //        await _context.SaveChangesAsync();  // Ensure that changes are saved to the database
    //    }
    //}
}
