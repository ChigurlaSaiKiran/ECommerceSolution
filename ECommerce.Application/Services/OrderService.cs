using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }

        //public async Task<IEnumerable<OrderDto>> GetAllAsync()
        //{
        //    var orders = await _orderRepository
        //        .GetAllAsync(include: o => o.Include(x => x.OrderItems));

        //    return orders.Select(o => new OrderDto
        //    {
        //        OrderId = o.Id,
        //        CustomerId = o.CustomerId,
        //        OrderDate = o.OrderDate,
        //        TotalAmount = o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
        //        OrderItems = o.OrderItems.Select(oi => new OrderItemDto
        //        {
        //            OrderItemId = oi.Id,
        //            ProductId = oi.ProductId,
        //            Quantity = oi.Quantity,
        //            UnitPrice = oi.UnitPrice
        //            // TotalPrice is computed in DTO
        //        }).ToList()
        //    });
        //}


        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository
                .GetAllAsync(include: o => o
                    .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Product)   // ✅ Include related Product
                );

            return orders.Select(o => new OrderDto
            {
                OrderId = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
               // TotalAmount = o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,  // ✅ MAP product name here
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            });
        }

        //public async Task<OrderDto?> GetByIdAsync(int id)
        //{
        //    var order = await _orderRepository.GetByIdAsync(
        //        id,
        //         include: o => o.Include(x => x.OrderItems) // ✅ include OrderItems!
        //    );

        //    if (order == null) return null;

        //    return new OrderDto
        //    {
        //        OrderId = order.Id,
        //        CustomerId = order.CustomerId,
        //        OrderDate = order.OrderDate,
        //        TotalAmount = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
        //        OrderItems = order.OrderItems.Select(oi => new OrderItemDto
        //        {
        //            OrderItemId = oi.Id,
        //            ProductId = oi.ProductId,
        //            Quantity = oi.Quantity,
        //            UnitPrice = oi.UnitPrice
        //        }).ToList()
        //    };
        //}
        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id, include: o =>
                o.Include(x => x.OrderItems).ThenInclude(oi => oi.Product));

            if (order == null)
                return null;

            return new OrderDto
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                //TotalAmount = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name, // ✅
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
        }

        public async Task AddAsync(OrderDto dto)
        {
            try
            {
                _logger.LogInformation("Starting to add new order for CustomerId: {CustomerId}", dto.CustomerId);

                var order = new Order
                {
                    CustomerId = dto.CustomerId,
                    OrderDate = dto.OrderDate,
                    OrderItems = dto.OrderItems.Select(oi => new OrderItem
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                };

                await _orderRepository.AddAsync(order); // ✅ Actually save it!
                _logger.LogInformation("Order and OrderItems saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding order");
                throw;
            }
        }

        public async Task UpdateAsync(OrderDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(dto.OrderId, include: o => o.Include(x => x.OrderItems));
            if (order == null) return;

            order.OrderDate = dto.OrderDate;
            order.CustomerId = dto.CustomerId;

            _orderRepository.Update(order);

            foreach (var item in dto.OrderItems)
            {
                var existingItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == item.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity = item.Quantity;
                    existingItem.UnitPrice = item.UnitPrice;
                    _orderItemRepository.Update(existingItem);
                }
                else
                {
                    var newOrderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    await _orderItemRepository.AddAsync(newOrderItem);
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id, include: o => o.Include(x => x.OrderItems));
            if (order != null)
            {
                foreach (var item in order.OrderItems)
                {
                    _orderItemRepository.Delete(item);
                }
                _orderRepository.Delete(order);
            }
        }
    }
}




//using ECommerce.Application.DTOs;
//using ECommerce.Application.Interfaces;
//using ECommerce.Domain.Entities;
//using ECommerce.Domain.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace ECommerce.Application.Services
//{
//    public class OrderService : IOrderService
//    {
//        private readonly IRepository<Order> _orderRepository;
//        private readonly IRepository<OrderItem> _orderItemRepository;
//        private readonly ILogger<OrderService> _logger; // Declare the logger field

//        public OrderService(IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository , ILogger<OrderService> logger)
//        {
//            _orderRepository = orderRepository;
//            _orderItemRepository = orderItemRepository;
//            _logger = logger; // Inject the logger through the constructor
//        }

//        public async Task<IEnumerable<OrderDto>> GetAllAsync()
//        {
//            var orders = await _orderRepository
//                .GetAllAsync(include: o => o.Include(x => x.OrderItems));

//            return orders.Select(o => new OrderDto
//            {
//                OrderId = o.Id,
//                OrderDate = o.OrderDate,
//                CustomerId = o.CustomerId,
//                //OrderItems = o.OrderItems != null
//                 TotalAmount = o.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
//                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
//                {
//                        ProductId = oi.ProductId,
//                        Quantity = oi.Quantity,
//                        UnitPrice = oi.UnitPrice,
//                            // TotalPrice = oi.UnitPrice * oi.Quantity
//                      }).ToList()
//                    //: new List<OrderItemDto>()
//            });
//        }



//        public async Task<OrderDto?> GetByIdAsync(int id)
//        {
//            var order = await _orderRepository.GetByIdAsync(id);
//            if (order == null) return null;

//            return new OrderDto
//            {
//                OrderId = order.Id,
//                OrderDate = order.OrderDate,
//                CustomerId = order.CustomerId,
//                OrderItems = order.OrderItems?.Select(oi => new OrderItemDto
//                {
//                    ProductId = oi.ProductId,
//                    Quantity = oi.Quantity,
//                    UnitPrice = oi.UnitPrice
//                }).ToList() ?? new List<OrderItemDto>()
//            };
//        }

//        public async Task AddAsync(OrderDto dto)
//        {
//            try
//            {
//                // Create the Order object
//                _logger.LogInformation("Starting to add new order for CustomerId: {CustomerId}", dto.CustomerId);
//                var order = new Order
//                {
//                    CustomerId = dto.CustomerId,
//                    OrderDate = dto.OrderDate,
//                    OrderItems = dto.OrderItems.Select(oi => new OrderItem
//                    {
//                        ProductId = oi.ProductId,
//                        Quantity = oi.Quantity,
//                        UnitPrice = oi.UnitPrice
//                    }).ToList()
//                };

//                // Save the Order to the database
//                //await _orderRepository.AddAsync(order);
//                var total = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
//                // Log successful insertion
//                _logger.LogInformation("Order and OrderItems saved successfully.");
//            }


//            catch (Exception ex)
//            {
//                // Log error
//                _logger.LogError($"Error occurred while adding order: {ex.Message}");
//                throw;
//            }
//        }


//        public async Task UpdateAsync(OrderDto dto)
//        {
//            var order = await _orderRepository.GetByIdAsync(dto.OrderId);
//            if (order == null) return;

//            order.OrderDate = dto.OrderDate;
//            order.CustomerId = dto.CustomerId;

//            _orderRepository.Update(order);

//            // Update or add order items
//            foreach (var item in dto.OrderItems)
//            {
//                var existingItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == item.ProductId);
//                if (existingItem != null)
//                {
//                    existingItem.Quantity = item.Quantity;
//                    existingItem.UnitPrice = item.UnitPrice;
//                    _orderItemRepository.Update(existingItem);
//                }
//                else
//                {
//                    var newOrderItem = new OrderItem
//                    {
//                        OrderId = order.Id,
//                        ProductId = item.ProductId,
//                        Quantity = item.Quantity,
//                        UnitPrice = item.UnitPrice
//                    };
//                    await _orderItemRepository.AddAsync(newOrderItem);
//                }
//            }
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var order = await _orderRepository.GetByIdAsync(id);
//            if (order != null)
//            {
//                foreach (var item in order.OrderItems)
//                {
//                    _orderItemRepository.Delete(item);
//                }
//                _orderRepository.Delete(order);
//            }
//        }
//    }
//}
