using System.Net.Mail;

namespace PublishSystem.Integration.Notification
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string from, List<string> to, List<string> cc, List<string> bcc, string subject, Attachment attachment, string body = "",  CancellationToken cancellationToken = default);
    }
}