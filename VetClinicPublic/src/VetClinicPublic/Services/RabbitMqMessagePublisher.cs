using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.ObjectPool;
using NArdalis.GuardClauses;
using RabbitMQ.Client;
using SharedKernel;
using VetClinicPublic.Interfaces;
using VetClinicPublic.Models;

namespace VetClinicPublic.Services
{
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly DefaultObjectPool<IModel> _objectPool;

        public RabbitMqMessagePublisher(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        public void Publish(AppointmentConfirmLinkClickedIntegrationEvent eventToPublish)
        {
            Guard.Against.Null(eventToPublish, nameof(eventToPublish));

            var channel = _objectPool.Get();
            var message = eventToPublish as object;

            try
            {
                var exchageName = MessagingConstants.Exchanges.FRONTDESK_VETCLINICPUBLIC_EXCHANGE;
                channel.ExchangeDeclare(exchange: exchageName,
                                        type: "direct",
                                        durable: true,
                                        autoDelete: false,
                                        arguments: null);

                var serialized = JsonSerializer.Serialize(message);
                var bytes = Encoding.UTF8.GetBytes(serialized);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: exchageName,
                                     routingKey: "appointment-confirmation",
                                     basicProperties: properties,
                                     body: bytes);
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }
    }
}
