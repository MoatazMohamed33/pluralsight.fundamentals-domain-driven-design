using VetClinicPublic.Models;

namespace VetClinicPublic.Interfaces
{
    public interface IMessagePublisher
    {
        void Publish(AppointmentConfirmLinkClickedIntegrationEvent eventToPublish);
    }
}
