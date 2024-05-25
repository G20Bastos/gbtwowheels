using System;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order GetById(int id);
        Task<ServiceResponse<Order>> Add(Order order);
        void Update(Order order);
        void Delete(int id);
    }
}

