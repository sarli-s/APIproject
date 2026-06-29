using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servers;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IPrudectsService _prudectsService;

        public ProductsController(IPrudectsService prudectsService)
        {
            _prudectsService = prudectsService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PageResponseDTO<ProductDTO>>> Get(
            string? description, int? minPrice, int? maxPrice,
            [FromQuery] int[]? categoriesId, int? limit, string? orderby, int offset = 1)
        {
            var result = await _prudectsService.GetProducts(description, minPrice, maxPrice, categoriesId, limit, orderby, offset);
            if (result != null) return Ok(result);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _prudectsService.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [AuthorizeRoles("Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create([FromBody] ProductDTO productDto)
        {
            var created = await _prudectsService.AddProduct(productDto);
            return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
        }

        [AuthorizeRoles("Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> Update(int id, [FromBody] ProductDTO productDto)
        {
            if (id != productDto.ProductId) return BadRequest();
            var updated = await _prudectsService.UpdateProduct(id, productDto);
            return Ok(updated);
        }

        [AuthorizeRoles("Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _prudectsService.GetProductById(id);
            if (product == null) return NotFound();
            await _prudectsService.DeleteProduct(id);
            return NoContent();
        }
    }
}
