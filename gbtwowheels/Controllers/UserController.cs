using Microsoft.AspNetCore.Mvc;
using gbtwowheels.Models;
using gbtwowheels.Interfaces;
using Microsoft.Extensions.Logging;

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

        // GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return _userService.GetAllUsers().ToList();
        }

        // GET: api/User/1
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

        //POST: api/user/addUser
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
