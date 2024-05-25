using Microsoft.AspNetCore.Mvc;
using gbtwowheels.Models;
using gbtwowheels.Interfaces;
using Microsoft.Extensions.Logging;
using gbtwowheels.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace gbtwowheels.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        private bool ValidateToken(out int userId)
        {
            userId = 0;
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var validatedUserId = JwtService.ValidateToken(token);
            if (validatedUserId.HasValue)
            {
                userId = validatedUserId.Value;
                return true;
            }
            return false;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            return _userService.GetAllUsers().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddUser([FromForm] User user)
        {
         
            var response = new ServiceResponse<User>();
            try
            {
                response = await _userService.AddUser(user);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the AddUser request.");
                return StatusCode(500, response.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User user)
        {
            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            _userService.UpdateUser(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.DeleteUser(id);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(User request)
        {
            var response = new ServiceResponse<User>();
            try
            {
                response = await _userService.Login(request);

                if (response.Success)
                {
                    return Ok(response);
                }

                return Unauthorized(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Login request.");
                return StatusCode(500, response.Message);
            }
        }
    }
}
