using System;
using gbtwowheels.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using gbtwowheels.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using RabbitMQ.Client.Exceptions;

namespace gbtwowheels.Helpers
{
    public class RabbitMQConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _rabbitMqHost;
        private readonly string _rabbitMqUser;
        private readonly string _rabbitMqPass;
        private readonly int _rabbitMqPort;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
            _rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
            _rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
            _rabbitMqPort = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672");
        }

        public async Task StartAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqHost,
                UserName = _rabbitMqUser,
                Password = _rabbitMqPass,
                Port = _rabbitMqPort
            };

            // Retry logic
            var maxRetries = 10;
            var delay = TimeSpan.FromSeconds(5);
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    SetupConsumer();
                    Console.WriteLine("Connected to RabbitMQ");
                    return; // Exit the method if connection is successful
                }
                catch (BrokerUnreachableException)
                {
                    if (i == maxRetries - 1) throw;
                    Console.WriteLine($"RabbitMQ connection failed. Retrying in {delay.TotalSeconds} seconds...");
                    await Task.Delay(delay);
                }
            }
        }

        private void SetupConsumer()
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
                        .Where(u => u.EndRentDate >= DateTime.Now && !context.Orders.Any(o => o.UserDeliveryId == u.UserId && o.StatusOrderId == 1 && o.OrderFinishDate == null))
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
