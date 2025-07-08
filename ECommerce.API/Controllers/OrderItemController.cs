using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        // GET: api/orderitem
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _orderItemService.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // GET: api/orderitem/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _orderItemService.GetByIdAsync(id);
                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // POST: api/orderitem
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderItemDto itemDto)
        {
            try
            {
                var createdItem = await _orderItemService.AddAsync(itemDto);
                return CreatedAtAction(nameof(GetById), new { id = createdItem.OrderItemId }, createdItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // PUT: api/orderitem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderItemDto itemDto)
        {
            if (id != itemDto.OrderItemId)
                return BadRequest("ID mismatch between URL and payload");

            try
            {
                await _orderItemService.UpdateAsync(itemDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // DELETE: api/orderitem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderItemService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
