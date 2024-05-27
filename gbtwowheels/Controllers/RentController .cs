using System;
using gbtwowheels.Filters;
using gbtwowheels.Helpers;
using gbtwowheels.Interfaces;
using gbtwowheels.Models;
using gbtwowheels.Services;
using Microsoft.AspNetCore.Mvc;

namespace gbtwowheels.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentController : ControllerBase
    {
        private readonly IRentService _rentService;

        private readonly ILogger<RentController> _logger;

        public RentController(IRentService rent, ILogger<RentController> logger)
        {
            _rentService = rent;
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



        // GET: api/Rent
        [HttpGet]
        public ActionResult<IEnumerable<Rent>> GetRents()
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            return _rentService.GetAllRents().ToList();
        }

        //POST: api/rent/addRent
        [HttpPost("[action]")]
        public async Task<IActionResult> AddRent(Rent rent)
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            var response = new ServiceResponse<Rent>();
            try
            {

                response = await _rentService.Add(rent);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while processing the AddRent request.");
                return StatusCode(500, response.Message);

            }

        }
    }
}

