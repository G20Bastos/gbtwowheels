﻿using System;
using gbtwowheels.Helpers;
using gbtwowheels.Interfaces;
using gbtwowheels.Models;
using Microsoft.AspNetCore.Mvc;

namespace gbtwowheels.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleService _motorcycleService;

        private readonly ILogger<MotorcycleController> _logger;

        public MotorcycleController(IMotorcycleService motorcycleService, ILogger<MotorcycleController> logger)
        {
            _motorcycleService = motorcycleService;
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



        // GET: api/Motorcycle
        [HttpGet]
        public ActionResult<IEnumerable<Motorcycle>> GetMotorcycles()
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            return _motorcycleService.GetAllMotorcycles().ToList();
        }

        // GET: api/Motorcycle/1
        [HttpGet("{id}")]
        public ActionResult<Motorcycle> GetMotorcycle(int id)
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            var motorcycle = _motorcycleService.GetMotorcycleById(id);

            if (motorcycle == null)
            {
                return NotFound();
            }

            return motorcycle;
        }

        //POST: api/motorcycle/addMotorcycle
        [HttpPost("[action]")]
        public async Task<IActionResult> AddMotorcycle(Motorcycle motorcycle)
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            var response = new ServiceResponse<Motorcycle>();
            try
            {

                response = await _motorcycleService.Add(motorcycle);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while processing the AddMotorcycle request.");
                return StatusCode(500, response.Message);

            }

        }


        // PUT: api/Motorcycle/5
        [HttpPut("{id}")]
        public IActionResult PutMotorcycle(int id, Motorcycle motorcycle)
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }


            if (id != motorcycle.MotorcycleId)
            {
                return BadRequest();
            }

            _motorcycleService.UpdateMotorcycle(motorcycle);

            return NoContent();
        }

        // DELETE: api/Motorcycle/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMotorcycle(int id)
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }


            var motorcycle = _motorcycleService.GetMotorcycleById(id);

            if (motorcycle == null)
            {
                return NotFound();
            }

            _motorcycleService.DeleteMotorcycle(id);

            return NoContent();
        }

      
    }
}

