using System;
using gbtwowheels.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using gbtwowheels.Models;
using Microsoft.Extensions.DependencyInjection;

namespace gbtwowheels.Helpers
{
    public class RabbitMQConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQConsumer(IServiceScopeFactory scopeFactory)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _scopeFactory = scopeFactory;
        }

        public void Start()
        {
            _channel.QueueDeclare(queue: "OrderNotifications",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var orderId = int.Parse(message.Split(' ')[1]);

                    var usersAvailable = context.Rents
                        .Where(u => u.EndRentDate >= DateTime.Now && !context.Orders.Any(o => o.UserDeliveryId == u.UserId && o.StatusOrderId == 1))
                        .ToList();

                    foreach (var userDelivery in usersAvailable)
                    {
                        var notification = new OrderNotification
                        {
                            OrderId = orderId,
                            UserId = userDelivery.UserId,
                            Message = message,
                            CreatedAt = DateTime.Now
                        };

                        context.OrderNotifications.Add(notification);
                    }

                    await context.SaveChangesAsync();
                }
            };

            _channel.BasicConsume(queue: "OrderNotifications",
                                  autoAck: true,
                                  consumer: consumer);
        }
    }
}
