using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                CategoryId = c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                CategoryId = category.Id,
                Name = category.Name
            };
        }

        public async Task AddAsync(CategoryDto dto)
        {
            var category = new Category { Name = dto.Name };
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null) return;

            category.Name = dto.Name;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                _categoryRepository.Delete(category);
                await _categoryRepository.SaveChangesAsync();
            }
        }
    }
}
