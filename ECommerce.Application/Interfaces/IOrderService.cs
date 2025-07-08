using ECommerce.Application.DTOs;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task AddAsync(OrderDto dto);
        Task UpdateAsync(OrderDto dto);
        Task DeleteAsync(int id);
    }
}
