using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Servers;
using DTOs;
using Repository;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIShop.Controllers
{

   
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        private readonly IUserService _userService;
        
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger= logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id) 
        {
            UserDTO user = await _userService.GetUserById(id);
            if(user!= null)
            {
                return Ok(user);
            }
            return NotFound();
        }
  
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserDTO user,string password)
        {
            UserDTO createdUser = await _userService.AddUser(user, password);
            if(createdUser!=null)
                return CreatedAtAction(nameof(Get), new{id = createdUser.UserId}, createdUser);
            return BadRequest("Password is not strong enough");
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginUserDTO loginUser)
        {
            UserDTO user = await _userService.Login(loginUser.UserEmail,loginUser.UserPassword);
            if (user != null)
            {
                _logger.LogInformation("Login attempted with User Name, {0} \n and password, {1}", loginUser.UserEmail, loginUser.UserPassword);
                return Ok(user);
            }
            return NoContent();
            //return Unauthorized();
        }
       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDTO user, string password)
        {
            bool isUpdateSuccessful = await _userService.UpdateUser(id, user, password);
            if (!isUpdateSuccessful)
            {
                return BadRequest("Password is not strong enough");
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userService.DeleteUser(id);
        }
    }
}
