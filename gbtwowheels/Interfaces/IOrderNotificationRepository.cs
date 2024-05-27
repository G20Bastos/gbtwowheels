using System;
using gbtwowheels.Filters;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IOrderNotificationRepository
    {
        
        IEnumerable<OrderNotification> GetAllOrderNotificationByUser(int userId);
        IEnumerable<OrderNotification> GetAllOrderNotification();
    }
}

