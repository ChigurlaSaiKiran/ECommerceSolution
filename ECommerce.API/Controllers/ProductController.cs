using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    //[Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts() =>
            Ok(await _productService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddAsync(dto);                 
                return Ok(new { message = "Product created." });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDto dto)
        {
            if (id != dto.ProductId)
                return BadRequest();

            await _productService.UpdateAsync(dto);
            return Ok(new { message = "Product updated." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok(new { message = "Product deleted." });
        }
    }
}
