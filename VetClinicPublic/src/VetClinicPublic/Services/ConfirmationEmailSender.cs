using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NArdalis.GuardClauses;
using VetClinicPublic.Configuration;
using VetClinicPublic.Interfaces;
using VetClinicPublic.Models;

namespace VetClinicPublic.Services
{
    public class ConfirmationEmailSender : ISendConfirmationEmail
    {
        private readonly ILogger<ConfirmationEmailSender> _logger;
        private readonly SiteConfiguration _siteConfiguration;
        private readonly ISendEmail _mailSender;

        public ConfirmationEmailSender(ILogger<ConfirmationEmailSender> logger,
                                       IOptions<SiteConfiguration> siteConfiguration,
                                       ISendEmail mailSender)
        {
            _logger = logger;
            _siteConfiguration = Guard.Against.Null(siteConfiguration?.Value, nameof(siteConfiguration));
            _mailSender = mailSender;
        }

        public void SendConfirmationEmail(SendAppointmentConfirmationCommand appointment)
        {
            _logger.LogInformation("Sending email to confirm appointment: {Appointment}", appointment);

            // TODO: Confirmation URL should be set in `appsettings.json`
            string confirmUrl = $"http://localhost:{_siteConfiguration.Port}/appointment/confirm/{appointment.AppointmentId}";
            const string from = "donotreply@thevetclinic.com";
            string to = appointment.ClientEmailAddress;
            string subject = $"Vet Appointment Confirmation for {appointment.PatientName}";
            string body = $@"
<html>
<body>
    Dear {appointment.ClientName},<br/>
    <p>
        Please click on the link below to confirm {appointment.PatientName}'s appointment for a {appointment.AppointmentType}
        with {appointment.DoctorName} on {appointment.AppointmentStart}.
    </p>
    <a href=""{confirmUrl}"">Confirm</a>
    <p>Please call the office to reschedule if you will be unable to make it for your appointment.</p>
    <p>Have a great day!</p>
    <hr/>
    <p>Debug: <code>{appointment.AppointmentId}</code></p>
</body>
</html>";

            _mailSender.SendEmail(to, from, subject, body);
        }
    }
}
