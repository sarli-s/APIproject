using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servers;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [AuthorizeRoles("Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
        {
            var orders = await _ordersService.GetAllOrders();
            return orders != null ? Ok(orders) : NoContent();
        }

        [AuthorizeRoles("Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> Get(int id)
        {
            OrderDTO order = await _ordersService.GetOrderById(id);
            if (order != null) return Ok(order);
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetByUser(int userId)
        {
            var orders = await _ordersService.GetOrdersByUserId(userId);
            return orders != null ? Ok(orders) : NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Post([FromBody] OrderDTO order)
        {
            OrderDTO createdOrder = await _ordersService.AddOrder(order);
            if (createdOrder != null)
                return CreatedAtAction(nameof(Get), new { id = createdOrder.userId }, createdOrder);
            return BadRequest("Order was not accepted.");
        }

        [AuthorizeRoles("Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var updated = await _ordersService.UpdateOrderStatus(id, status);
            if (updated) return Ok();
            return BadRequest("Could not update status.");
        }
    }
}
