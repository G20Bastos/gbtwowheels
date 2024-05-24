using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using gbtwowheels.Models;
using gbtwowheels.Services;
using NuGet.Protocol.Plugins;

namespace gbtwowheels.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return _userService.GetAllUsers().ToList();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            var response = await _userService.AddUser(user);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _userService.UpdateUser(user);

            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.DeleteUser(id);

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User request)
        {
            var result = await _userService.Login(request);
            if (result.Success)
            {
                return Ok(result); 
            }
            return Unauthorized(result); 
        }
    }
}
