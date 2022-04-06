using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublishSystem.Domain.Entities;
using PublishSystem.Domain.Models;
using PublishSystem.Domain.SeedWork;
using PublishSystem.Domain.Specification.EmailTEmplateSpecification;

namespace PublishSystem.Domain.Repositories
{
    public class EmailTemplateRepository : RepositoryBase<EmailTemplateRepository, EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(ILogger<EmailTemplateRepository> logger, IMapper mapper, IUnitOfWork unitOfWork, IEntityRepository<EmailTemplate> entityRepository)
            : base(logger, mapper, entityRepository)
        {
        }

        public async Task<EmailTemplateModel?> GetEmailTemplateByCodeAsync(string code)
        {
            EmailTemplateModel? emailTemplateModel = null;
            var emailTemplateEntity = await GetByCodeAsync(code);
            if (emailTemplateEntity != null)
            {
                emailTemplateModel = _mapper.Map<EmailTemplate, EmailTemplateModel>(emailTemplateEntity);
            }
            return emailTemplateModel;
        }

        private async Task<EmailTemplate?> GetByCodeAsync(string code)
        {
            var emailTemplateEntity = await _entityRepository.Where(new EmailTemplateByCodeSpecification(code)).SingleOrDefaultAsync();
            return emailTemplateEntity;
        }
    }
}
