using ECommerce.Application.DTOs;

namespace ECommerce.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task AddAsync(CategoryDto dto);
        Task UpdateAsync(CategoryDto dto);
        Task DeleteAsync(int id);
    }
}
