using System;

namespace VetClinicPublic.Models
{
    public class AppointmentConfirmLinkClickedIntegrationEvent
    {
        public AppointmentConfirmLinkClickedIntegrationEvent(Guid appointmentId)
        {
            Id = Guid.NewGuid();
            AppointmentId = appointmentId;
            EventOccurredOn = DateTimeOffset.Now;
        }

        public Guid Id { get; private set; }
        public Guid AppointmentId { get; set; }
        public DateTimeOffset EventOccurredOn { get; set; }
        public string EventType => nameof(AppointmentConfirmLinkClickedIntegrationEvent);
    }
}
