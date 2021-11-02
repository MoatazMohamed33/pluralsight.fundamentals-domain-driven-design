using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NArdalis.GuardClauses;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel;
using VetClinicPublic.Configuration;
using VetClinicPublic.Models;

namespace VetClinicPublic.BackgroundServices
{
    public class FrontDeskRabbitMqService : BackgroundService
    {
        private readonly ILogger<FrontDeskRabbitMqService> _logger;
        private readonly RabbitMqConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private IModel _channel;
        private IConnection _connection;

        private readonly string _exchangeName = MessagingConstants.Exchanges.FRONTDESK_VETCLINICPUBLIC_EXCHANGE;
        private readonly string _queueIn = MessagingConstants.Queues.FDVCP_VETCLINICPUBLIC_IN;

        public FrontDeskRabbitMqService(ILogger<FrontDeskRabbitMqService> logger,
                                        IOptions<RabbitMqConfiguration> configuration,
                                        IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _configuration = Guard.Against.Null(configuration?.Value, nameof(configuration));
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Connection to RabbitMQ with {Configuration}", _configuration);

            try
            {
                var factory = new ConnectionFactory
                {
                    DispatchConsumersAsync = true,
                    HostName = _configuration.HostName,
                    Port = _configuration.Port,
                    UserName = _configuration.UserName,
                    Password = _configuration.Password,
                    VirtualHost = _configuration.VirtualHost
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: _exchangeName,
                                         type: "direct",
                                         durable: true,
                                         autoDelete: false,
                                         arguments: null);

                _channel.QueueDeclare(queue: _queueIn,
                                      durable: true,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                string routingKey = "appointment-scheduled";
                _channel.QueueBind(queue: _queueIn,
                                   exchange: _exchangeName,
                                   routingKey);

                _logger.LogInformation("Connected to RabbitMQ, listening for messages on {Exchange}-{RoutingKey}", _exchangeName, routingKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to RabbitMQ");
            }

            return base.StartAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            _channel.Dispose();
            _connection.Dispose();

            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += OnMessageReceived;

            _channel.BasicConsume(_queueIn,
                                  autoAck: true,
                                  consumer);

            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            try
            {
                await HandleMessage(message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling RabbitMQ message");
            }
        }

        private async Task HandleMessage(string message)
        {

            using var document = JsonDocument.Parse(message);
            var root = document.RootElement;
            var eventType = root.GetProperty("EventType");

            _logger.LogInformation("Handling RabbitMQ message for {EventType}", eventType);

            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            if (eventType.GetString() == "AppointmentScheduledIntegrationEvent")
            {
                var command = new SendAppointmentConfirmationCommand
                {
                    AppointmentId = root.GetProperty("AppointmentId").GetGuid(),
                    AppointmentType = root.GetProperty("AppointmentType").GetString(),
                    ClientEmailAddress = root.GetProperty("ClientEmailAddress").GetString(),
                    ClientName = root.GetProperty("ClientName").GetString(),
                    DoctorName = root.GetProperty("DoctorName").GetString(),
                    PatientName = root.GetProperty("PatientName").GetString(),
                    AppointmentStart = root.GetProperty("AppointmentStart").GetDateTime()
                };

                await mediator.Send(command);
            }
            else
            {
                throw new ArgumentException($"Unknown event type {eventType}");
            }
        }
    }
}
