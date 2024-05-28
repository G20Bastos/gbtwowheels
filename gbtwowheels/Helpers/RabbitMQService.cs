using RabbitMQ.Client;
using System.Text;
namespace gbtwowheels.Helpers
{
    public class RabbitMQService
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly string _rabbitMqHost;
        private readonly string _rabbitMqUser;
        private readonly string _rabbitMqPass;
        private readonly int _rabbitMqPort;

        public RabbitMQService(IServiceScopeFactory scopeFactory)
        {

            _scopeFactory = scopeFactory;
            _rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
            _rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
            _rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
            _rabbitMqPort = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672");

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqHost,
                UserName = _rabbitMqUser,
                Password = _rabbitMqPass,
                Port = _rabbitMqPort
            };
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

