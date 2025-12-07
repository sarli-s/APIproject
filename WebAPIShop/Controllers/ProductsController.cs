using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Servers;
using Entitys;
using Repository;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get(string? name, int? minPrice, int? maxPrice, int[]? categoriesId,
            int? limit, string? orderby, int offset=1) 
        {
            List<Product> products = await _prudectsService.GetProducts(name,minPrice,maxPrice,categoriesId,limit,orderby,offset);
            if(products != null)
            {
                return Ok(products);
            }
            return NotFound();
        }
    }
}
