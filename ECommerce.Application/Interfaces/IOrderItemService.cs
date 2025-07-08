using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> GetAllAsync();
        Task<OrderItemDto> GetByIdAsync(int id);
        Task<OrderItemDto> AddAsync(OrderItemDto dto);
        Task UpdateAsync(OrderItemDto dto);
        Task DeleteAsync(int id);
    }
}
