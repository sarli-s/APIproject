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
    public class OrdersController : ControllerBase
    {

        private readonly IOrdersService _ordersService;
        
        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id) 
        {
            Order order = await _ordersService.GetOrderById(id);
            if(order != null)
            {
                return Ok(order);
            }
            return NoContent();
        }
  
        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] Order order)
        {
            Order createdOrder = await _ordersService.AddOrder(order);
            if(createdOrder != null)
                return CreatedAtAction(nameof(Get), new{id = createdOrder.UserId}, createdOrder);
            return BadRequest("order d'ont eccept!!");
        }


       
    }
}
