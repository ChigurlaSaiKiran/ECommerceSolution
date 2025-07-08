using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerce.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include);

        Task<T?> GetByIdAsync(int id);

        Task<T?> GetByIdAsync(
            int id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include); // ✅ Add this

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
    //public interface IRepository<T> where T : class
    //{
    //    Task<IEnumerable<T>> GetAllAsync();
    //    Task<IEnumerable<T>> GetAllAsync(
    //  Func<IQueryable<T>, IIncludableQueryable<T, object>> include);
    //    Task<T> GetByIdAsync(int id);
    //    Task AddAsync(T entity);
    //    void Update(T entity);
    //    void Delete(T entity);
    //    Task SaveChangesAsync();
    //}


}
