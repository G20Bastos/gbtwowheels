using RabbitMQ.Client;
using System.Text;
namespace gbtwowheels.Helpers
{
    public class RabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "OrderNotifications",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                  routingKey: "OrderNotifications",
                                  basicProperties: null,
                                  body: body);
        }
    }
}

