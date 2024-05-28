using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace gbtwowheels.Helpers
{
    public class RabbitMQConsumerHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private RabbitMQConsumer _rabbitMQConsumer;

        public RabbitMQConsumerHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _rabbitMQConsumer = scope.ServiceProvider.GetRequiredService<RabbitMQConsumer>();
                await _rabbitMQConsumer.StartAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Implement any cleanup logic here if necessary
            return Task.CompletedTask;
        }
    }
}
