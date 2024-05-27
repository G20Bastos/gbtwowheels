using System;
using gbtwowheels.Filters;
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
        IEnumerable<Order> GetByFilter(OrderFilters filters);
        IEnumerable<Order> GetAllOrderLinkedByUser(int userId);
    }
}

