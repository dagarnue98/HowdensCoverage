namespace PublishSystem.Application.Services.EmailService
{
    using System.Dynamic;
    using AutoMapper;
    using HandlebarsDotNet;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using PublishSystem.Domain.Models;
    using PublishSystem.Domain.Repositories;
    using PublishSystem.Domain.SeedWork;
    using PublishSystem.Integration.Messaging;

    public class EmailService : ServiceBase<EmailService>, IEmailService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public EmailService(IEmailTemplateRepository emailTemplateRepository,
            ILogger<EmailService> logger, IMapper mapper, IUnitOfWork unitOfWork, IEventBus eventBus)
            : base(logger, mapper, unitOfWork, eventBus)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }

        public async Task<LayerResponse<string>> GetEmailBodyTranslatedAsync(string code, string language)
        {
            await _unitOfWork.BeginTransactionAsync();
            var emailTemplateModel = await _emailTemplateRepository.GetEmailTemplateByCodeAsync(code);
            await _unitOfWork.CommitAsync();

            if (emailTemplateModel == null)
            {
                throw new ApplicationException($"Email template with code {code} does not exist.");
            }

            dynamic result = TranslateEmailBody(language, emailTemplateModel);
            return new LayerResponse<string>(result);
        }

        private static dynamic TranslateEmailBody(string lenguage, EmailTemplateModel? emailTemplateModel)
        {
            dynamic dataTranslated = new ExpandoObject();
            dataTranslated.Data = new EmailDataModel();
            dataTranslated.Translate = JArray.Parse(emailTemplateModel.Translate)
                .Children<JObject>().Properties().FirstOrDefault(x => x.Name.ToLower() == lenguage.ToLower()).Value;
            var eo = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(dataTranslated));

            var result = Handlebars.Compile(emailTemplateModel.CustomTemplate).Invoke(eo);
            return result;
        }
    }
}
