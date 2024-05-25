using System;
using gbtwowheels.Controllers;
using gbtwowheels.Interfaces;
using gbtwowheels.Models;
using gbtwowheels.Repositories;
using gbtwowheels.Utils;

namespace gbtwowheels.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Order>> Add(Order order)
        {
            var response = new ServiceResponse<Order>();

            try
            {

                                   
                    var result = await _orderRepository.Add(order);
                    response.Success = true;
                    response.Message = "Pedido cadastrado com sucesso!";
                    response.Data = result.Data;

                    
                


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error to add order in service");


                response.Success = false;
                response.Message = "Ocorreu um erro ao adicionar o pedido na plataforma.";

            }

            return response;
        }

        public void DeleteOrder(int id)
        {
            _orderRepository.Delete(id);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAll();
        }

        public Order GetOrderById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public void UpdateOrder(Order order)
        {
            _orderRepository.Update(order);
        }
    }
}

