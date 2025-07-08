using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
     [Authorize] 
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ✅ GET: api/orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // ✅ GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // ✅ POST: api/orders
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDto dto)
        {
            await _orderService.AddAsync(dto);
            return Ok(new { message = "Order created successfully!" });
        }

        // ✅ PUT: api/orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderDto dto)
        {
            if (id != dto.OrderId)
                return BadRequest("ID mismatch.");

            await _orderService.UpdateAsync(dto);
            return Ok(new { message = "Order updated successfully!" });
        }

        // ✅ DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteAsync(id);
            return Ok(new { message = "Order deleted successfully!" });
        }
    }
}











//using ECommerce.Application.DTOs;
//using ECommerce.Application.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace ECommerce.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize] 
//    public class OrderController : ControllerBase
//    {
//        private readonly IOrderService _orderService;

//        public OrderController(IOrderService orderService)
//        {
//            _orderService = orderService;
//        }

//        // GET: api/order
//        [HttpGet]
//        public async Task<IActionResult> GetAllOrders()
//        {
//            var orders = await _orderService.GetAllAsync();
//            return Ok(orders);
//        }

//        // GET: api/order/5
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetOrderById(int id)
//        {
//            var order = await _orderService.GetByIdAsync(id);
//            if (order == null)
//                return NotFound();

//            return Ok(order);
//        }

//        // POST: api/order
//        [HttpPost]
//        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            await _orderService.AddAsync(orderDto);
//            return CreatedAtAction(nameof(GetOrderById), new { id = orderDto.OrderId }, orderDto);
//        }

//        // PUT: api/order/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
//        {
//            if (id != orderDto.OrderId)
//                return BadRequest("Order ID mismatch");

//            await _orderService.UpdateAsync(orderDto);
//            return NoContent();
//        }

//        // DELETE: api/order/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteOrder(int id)
//        {
//            await _orderService.DeleteAsync(id);
//            return NoContent();
//        }
//    }
//}
