using VetClinicPublic.Models;

namespace VetClinicPublic.Interfaces
{
    public interface ISendConfirmationEmail
    {
        void SendConfirmationEmail(SendAppointmentConfirmationCommand appointment);
    }
}
