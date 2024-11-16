namespace ChoicesExtrasManagement.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync (string email, string subject, string message);
        Task SendEmailSelfAsync(string subject, string message);
    }
}
