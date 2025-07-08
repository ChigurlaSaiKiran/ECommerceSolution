using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly ILogger<OrderItemService> _logger;

        public OrderItemService(IRepository<OrderItem> orderItemRepository, ILogger<OrderItemService> logger)
        {
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllAsync()
        {
            var items = await _orderItemRepository.GetAllAsync();
            return items.Select(i => new OrderItemDto
            {
                OrderItemId = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            });
        }

        public async Task<OrderItemDto?> GetByIdAsync(int id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);
            if (item == null) return null;

            return new OrderItemDto
            {
                OrderItemId = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            };
        }

        public async Task<OrderItemDto> AddAsync(OrderItemDto dto)
        {
            var orderItem = new OrderItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                OrderId = 0 // Ensure you set OrderId if needed, or pass via DTO
            };

            await _orderItemRepository.AddAsync(orderItem);

            _logger.LogInformation("Order item added: {ProductId}, Qty: {Quantity}", dto.ProductId, dto.Quantity);

            dto.OrderItemId = orderItem.Id;
            return dto;
        }

        public async Task UpdateAsync(OrderItemDto dto)
        {
            var existing = await _orderItemRepository.GetByIdAsync(dto.OrderItemId);
            if (existing == null) return;

            existing.ProductId = dto.ProductId;
            existing.Quantity = dto.Quantity;
            existing.UnitPrice = dto.UnitPrice;

            _orderItemRepository.Update(existing);

            _logger.LogInformation("Order item updated: {Id}", dto.OrderItemId);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);
            if (item != null)
            {
                _orderItemRepository.Delete(item);
                _logger.LogInformation("Order item deleted: {Id}", id);
            }
        }
    }
}
