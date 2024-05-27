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
using gbtwowheels.Filters;
using static NuGet.Packaging.PackagingConstants;

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

        public IEnumerable<Order> GetAllOrderLinkedByUser(int userId)
        {
            IQueryable<Order> query = _context.Orders;
            query = query.Where(o => o.UserDeliveryId == userId && o.OrderFinishDate == null);
            return query.ToList();
        }

        public IEnumerable<Order> GetByFilter(OrderFilters filters)
        {
            IQueryable<Order> query = _context.Orders;



            if (filters.StartDate.HasValue && filters.EndDate.HasValue)
            {
                var startDate = filters.StartDate.Value.Date;
                var endDate = filters.EndDate.Value.Date.AddDays(1).AddSeconds(-1);

                query = query.Where(m => m.OrderCreationDate.Date >= startDate && m.OrderCreationDate.Date <= endDate);
            }

            else if (filters.StartDate.HasValue)
            {
                query = query.Where(m => m.OrderCreationDate >= filters.StartDate.Value);
            }
            else if (filters.EndDate.HasValue)
            {
                query = query.Where(m => m.OrderCreationDate <= filters.EndDate.Value);
            }


            if (!string.IsNullOrEmpty(filters.AddressOrder))
            {
                query = query.Where(m => m.AddressOrder!.Contains(filters.AddressOrder));
            }

            if (filters.OrderServiceValue.HasValue && filters.OrderServiceValue != 0)
            {
                query = query.Where(m => m.OrderServiceValue == filters.OrderServiceValue.Value);
            }

            if (filters.StatusOrderId.HasValue && filters.StatusOrderId != 0)
            {
                query = query.Where(m => m.StatusOrderId == filters.StatusOrderId);
            }


            return query.ToList();
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
