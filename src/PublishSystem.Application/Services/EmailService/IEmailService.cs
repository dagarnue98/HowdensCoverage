using PublishSystem.Domain.SeedWork;

namespace PublishSystem.Application.Services.EmailService
{
    public interface IEmailService : IServiceBase
    {
        Task<LayerResponse<string>> GetEmailBodyTranslatedAsync(string code, string language);
    }
}