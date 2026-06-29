using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servers;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatRequest req)
        {
            try
            {
                var data = await _chatService.SendMessageAsync(req);
                return Ok(data);
            }
            catch (Exception)
            {
                return StatusCode(500, "AI service unavailable");
            }
        }
    }
}
