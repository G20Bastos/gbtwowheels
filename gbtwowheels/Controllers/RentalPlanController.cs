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
    public class RentalPlanController : ControllerBase
    {
        private readonly IRentalPlanService _rentalPlanService;

        private readonly ILogger<RentalPlanController> _logger;

        public RentalPlanController(IRentalPlanService rentalPlan, ILogger<RentalPlanController> logger)
        {
            _rentalPlanService = rentalPlan;
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



        // GET: api/RentalPlan
        [HttpGet]
        public ActionResult<IEnumerable<RentalPlan>> GetRentalPlans()
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            return _rentalPlanService.GetAllRentalPlans().ToList();
        }
    }
}

