using System;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
        Task<ServiceResponse<Order>> Add(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);

    }
}

