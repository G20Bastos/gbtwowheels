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
    public class OrderNotificationRepository : IOrderNotificationRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<OrderNotificationRepository> _logger;

        public OrderNotificationRepository(ApplicationDbContext context, ILogger<OrderNotificationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<OrderNotification> GetAllOrderNotificationByUser(int userId)
        {
            IQueryable<OrderNotification> query = _context.OrderNotifications;
            query = query.Where(o => o.UserId == userId);
            return query.ToList();
        }
    }
}
