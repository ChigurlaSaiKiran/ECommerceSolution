using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductDto
            {
                ProductId = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId
            });
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                ProductId = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            };
        }

        //public async Task AddAsync(ProductDto dto)
        //{
        //    var product = new Product
        //    {
        //        Name = dto.Name,
        //        Description = dto.Description,
        //        Price = dto.Price,
        //        Stock = dto.Stock,
        //        CategoryId = dto.CategoryId
        //    };
        //    await _productRepository.AddAsync(product);
        //}
        public async Task AddAsync(ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId
            };
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync(); // <-- THIS IS REQUIRED
        }
        public async Task UpdateAsync(ProductDto dto)
        {
            var existing = await _productRepository.GetByIdAsync(dto.ProductId);
            if (existing == null) return;

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;
            existing.CategoryId = dto.CategoryId;

            _productRepository.Update(existing);
            await _productRepository.SaveChangesAsync(); // <-- REQUIRED
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                _productRepository.Delete(product);
                await _productRepository.SaveChangesAsync(); // <-- REQUIRED
            }
        }


    }
}
