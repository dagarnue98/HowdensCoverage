namespace PublishSystem.Application.Test.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;
    using PublishSystem.Application.Events;
    using PublishSystem.Application.Services.EmailService;
    using PublishSystem.Application.Services.PublishJobService;
    using PublishSystem.Application.Services.RenderingService;
    using PublishSystem.Domain.BusinessRules.StateManager;
    using PublishSystem.Domain.Enums.StateManagement;
    using PublishSystem.Domain.Models;
    using PublishSystem.Domain.Repositories;
    using PublishSystem.Domain.SeedWork;
    using PublishSystem.Integration.Messaging;
    using Xunit;

    public class EmailServiceTest
    {
        [Fact]
        public void GetEmailBodyTranslatedAsyncSuccess()
        {
            //Arrange
            var emailTemplate = new EmailTemplateModel()
            {
                Name = "Welcome",
                Code = "W3H",
                CustomTemplate = "One: {{Translate.sentence1}} // Two: {{Translate.sentence2}}",
                Translate = "[{\"en\": {\"sentence1\": \"Hello there\",\"sentence2\": \"General Kenobi\"}},{\"fr\": {\"sentence1\": \"You're our progeny\",\"sentence2\": \"Our newest kinsman\"}}]",
            };

            var logger = NullLoggerFactory.Instance.CreateLogger<EmailService>();
            var emailTemplateRepository = new Mock<IEmailTemplateRepository>(MockBehavior.Strict);
            emailTemplateRepository.Setup(m => m.GetEmailTemplateByCodeAsync(It.IsAny<string>())).Returns(Task.FromResult(emailTemplate));
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            var unitOfWork = new Mock<IUnitOfWork>();
            var eventBus = new Mock<IEventBus>(MockBehavior.Strict);

            eventBus.Setup(m => m.SendAsync(It.IsAny<RenderingRequestEvent>(), default)).Returns(Task.CompletedTask);
            var emailService = new EmailService(emailTemplateRepository.Object, logger, mapper.Object, unitOfWork.Object, eventBus.Object);

            //Act
            var response = emailService.GetEmailBodyTranslatedAsync("W3H", "en");

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void GetEmailBodyTranslatedAsyncNullCodeFailed()
        {
            //Arrange
            var emailTemplate = new EmailTemplateModel()
            {
                Name = "Welcome",
                Code = "W3H",
                CustomTemplate = "One: {{Translate.sentence1}} // Two: {{Translate.sentence2}}",
                Translate = "[{\"en\": {\"sentence1\": \"Hello there\",\"sentence2\": \"General Kenobi\"}},{\"fr\": {\"sentence1\": \"You're our progeny\",\"sentence2\": \"Our newest kinsman\"}}]",
            };

            var logger = NullLoggerFactory.Instance.CreateLogger<EmailService>();
            var emailTemplateRepository = new Mock<IEmailTemplateRepository>(MockBehavior.Strict);
            emailTemplateRepository.Setup(m => m.GetEmailTemplateByCodeAsync(It.IsNotNull<string>())).Returns(Task.FromResult(emailTemplate));
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            var unitOfWork = new Mock<IUnitOfWork>();
            var eventBus = new Mock<IEventBus>(MockBehavior.Strict);

            var emailService = new EmailService(emailTemplateRepository.Object, logger, mapper.Object, unitOfWork.Object, eventBus.Object);

            //Act
            var response = emailService.GetEmailBodyTranslatedAsync(null, "en");

            //Assert
            Assert.True(response.IsFaulted);
        }

        [Fact]
        public void GetEmailBodyTranslatedAsyncNullLanguageailed()
        {
            //Arrange
            var emailTemplate = new EmailTemplateModel()
            {
                Name = "Welcome",
                Code = "W3H",
                CustomTemplate = "One: {{Translate.sentence1}} // Two: {{Translate.sentence2}}",
                Translate = "[{\"en\": {\"sentence1\": \"Hello there\",\"sentence2\": \"General Kenobi\"}},{\"fr\": {\"sentence1\": \"You're our progeny\",\"sentence2\": \"Our newest kinsman\"}}]",
            };

            var logger = NullLoggerFactory.Instance.CreateLogger<EmailService>();
            var emailTemplateRepository = new Mock<IEmailTemplateRepository>(MockBehavior.Strict);
            emailTemplateRepository.Setup(m => m.GetEmailTemplateByCodeAsync(It.IsNotNull<string>())).Returns(Task.FromResult(emailTemplate));
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            var unitOfWork = new Mock<IUnitOfWork>();
            var eventBus = new Mock<IEventBus>(MockBehavior.Strict);

            var emailService = new EmailService(emailTemplateRepository.Object, logger, mapper.Object, unitOfWork.Object, eventBus.Object);

            //Act
            var response = emailService.GetEmailBodyTranslatedAsync("W3H", null);

            //Assert
            Assert.True(response.IsFaulted);
        }
    }
}
