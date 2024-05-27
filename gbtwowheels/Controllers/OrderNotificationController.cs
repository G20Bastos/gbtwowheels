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
    public class OrderNotificationController : ControllerBase
    {
        private readonly IOrderNotificationService _orderNotificationService;

        private readonly ILogger<OrderNotificationController> _logger;

        public OrderNotificationController(IOrderNotificationService orderNotificationService, ILogger<OrderNotificationController> logger)
        {
            _orderNotificationService = orderNotificationService;
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



        // GET: api/OrderNotification/1
        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<OrderNotification>> GetByFilter(int userId)
        {


            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }
            try
            {

                return _orderNotificationService.GetAllOrderNotificationByUser(userId).ToList();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Não foram encontrados resultados com o id passado");
            }
        }


    }
}

