using System.Collections.Generic;
using System.Linq;
using gbtwowheels.Data;
using gbtwowheels.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using BCrypt.Net;
using gbtwowheels.Interfaces;
using Azure;
using gbtwowheels.Controllers;

namespace gbtwowheels.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ApplicationDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResponse<Order>> Add(Order order)
        {
            var response = new ServiceResponse<Order>();

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = order;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while add order in database");

            }

            return response;
        }

        public void Delete(int id)
        {
            var order = GetById(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders.Find(id);
        }

    

        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

       
    }
}
