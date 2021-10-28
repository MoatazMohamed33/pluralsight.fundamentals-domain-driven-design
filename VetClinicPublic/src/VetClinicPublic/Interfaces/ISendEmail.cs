namespace VetClinicPublic.Interfaces
{
    public interface ISendEmail
    {
        void SendEmail(string to, string from, string subject, string body);
    }
}
