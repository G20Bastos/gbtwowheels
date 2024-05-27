using System;
using gbtwowheels.Controllers;
using gbtwowheels.Filters;
using gbtwowheels.Helpers;
using gbtwowheels.Interfaces;
using gbtwowheels.Models;
using gbtwowheels.Repositories;
using gbtwowheels.Utils;

namespace gbtwowheels.Services
{
    public class OrderNotificationService : IOrderNotificationService
    {

        private readonly IOrderNotificationRepository _orderNotificationRepository;
        private readonly ILogger<OrderNotificationService> _logger;

        public OrderNotificationService(IOrderNotificationRepository orderNotificationRepository, ILogger<OrderNotificationService> logger)
        {
            _orderNotificationRepository = orderNotificationRepository;
            _logger = logger;
        }

        public IEnumerable<OrderNotification> GetAllOrderNotification()
        {
            var orderNotificationList = _orderNotificationRepository.GetAllOrderNotification();
            return orderNotificationList;
        }

        public IEnumerable<OrderNotification> GetAllOrderNotificationByUser(int userId)
        {
            return _orderNotificationRepository.GetAllOrderNotificationByUser(userId);
        }
    }
}

