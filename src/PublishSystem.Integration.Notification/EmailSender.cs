namespace PublishSystem.Integration.Notification
{
    using System.Net.Http.Headers;
    using System.Net.Mail;

    public class EmailSender : IEmailSender
    {
        private readonly string _integrationUrl;

        public EmailSender(string integrationUrl)
        {
            _integrationUrl = integrationUrl;
        }

        public async Task SendEmailAsync(string from, List<string> to, List<string> cc, List<string> bcc, string subject, Attachment attachment, string body, CancellationToken cancellationToken = default)
        {
            var client = new HttpClient();
            var reader = new StreamReader(attachment.ContentStream);
            var values = new Dictionary<string, string>
            {
                { "from", from },
                { "to", string.Join(",", to) },
                { "cc", string.Join(",", cc) },
                { "bcc", string.Join(",", bcc) },
                { "subject", subject },
                { "body", body },
                { "attachment", reader.ReadToEnd() },
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(_integrationUrl, content);

            await response.Content.ReadAsStringAsync();
        }
    }
}
