using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VetClinicPublic.Interfaces;
using VetClinicPublic.Models;

namespace VetClinicPublic.Services
{
    public class ConfirmationEmailHandler : IRequestHandler<SendAppointmentConfirmationCommand>
    {
        private readonly ILogger<ConfirmationEmailHandler> _logger;
        private readonly ISendConfirmationEmail _emailSender;

        public ConfirmationEmailHandler(ILogger<ConfirmationEmailHandler> logger,
                                        ISendConfirmationEmail emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public Task<Unit> Handle(SendAppointmentConfirmationCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Request received — Sending confirmation email for {AppointmentId}", request.AppointmentId);

            _emailSender.SendConfirmationEmail(request);

            return Task.FromResult(Unit.Value);
        }
    }
}
