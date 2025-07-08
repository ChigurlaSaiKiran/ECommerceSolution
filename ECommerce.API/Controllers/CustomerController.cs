using ECommerce.Application.DTOs;   
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerDto dto)
        {
            await _customerService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.CustomerId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CustomerDto dto)
        {
            if (id != dto.CustomerId) return BadRequest();
            await _customerService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteAsync(id);
            return NoContent();
        }
    }
}
