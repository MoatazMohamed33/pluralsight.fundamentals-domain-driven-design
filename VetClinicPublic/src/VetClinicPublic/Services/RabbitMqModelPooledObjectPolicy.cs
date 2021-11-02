using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using NArdalis.GuardClauses;
using RabbitMQ.Client;
using VetClinicPublic.Configuration;

namespace VetClinicPublic.Services
{
    public class RabbitMqModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly IConnection _connection;

        public RabbitMqModelPooledObjectPolicy(IOptions<RabbitMqConfiguration> rabbitMqConfiguration)
        {
            var configuration = Guard.Against.Null(rabbitMqConfiguration?.Value, nameof(rabbitMqConfiguration));
            _connection = GetConnection(configuration);
        }

        public IModel Create() => _connection.CreateModel();

        public bool Return(IModel obj)
        {
            if (obj is not null && obj.IsOpen)
            {
                return true;
            }

            obj?.Dispose();
            return false;
        }

        private IConnection GetConnection(RabbitMqConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.HostName,
                Port = configuration.Port,
                UserName = configuration.UserName,
                Password = configuration.Password,
                VirtualHost = configuration.VirtualHost
            };

            return factory.CreateConnection();
        }
    }
}
