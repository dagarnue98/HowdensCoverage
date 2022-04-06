namespace PublishSystem.Domain.Repositories
{
    using PublishSystem.Domain.Models;
    using PublishSystem.Domain.SeedWork;

    public interface IEmailTemplateRepository : IRepositoryBase
    {
        Task<EmailTemplateModel?> GetEmailTemplateByCodeAsync(string code);
    }
}