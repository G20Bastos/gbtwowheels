using System;
using gbtwowheels.Helpers;
using gbtwowheels.Interfaces;
using gbtwowheels.Models;
using Microsoft.AspNetCore.Mvc;

namespace gbtwowheels.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
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



        // GET: api/Order
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            return _orderService.GetAllOrders().ToList();
        }

        // GET: api/Order/1
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            var order = _orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        //POST: api/order/addOrder
        [HttpPost("[action]")]
        public async Task<IActionResult> AddOrder(Order order)
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }

            var response = new ServiceResponse<Order>();
            try
            {

                response = await _orderService.Add(order);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while processing the AddOrder request.");
                return StatusCode(500, response.Message);

            }

        }


        // PUT: api/Order/5
        [HttpPut("{id}")]
        public IActionResult PutOrder(int id, Order order)
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }


            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _orderService.UpdateOrder(order);

            return NoContent();
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {

            if (!ValidateToken(out _))
            {
                return Unauthorized("Invalid token");
            }


            var order = _orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            _orderService.DeleteOrder(id);

            return NoContent();
        }

      
    }
}

