namespace PublishSystem.Domain.Test.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using MockQueryable.Moq;
    using Moq;
    using PublishSystem.Domain.Entities;
    using PublishSystem.Domain.Models;
    using PublishSystem.Domain.Repositories;
    using PublishSystem.Domain.SeedWork;
    using Xunit;

    public class EmailTemplateRepositoryTest
    {
        [Fact]
        public void GetEmailTemplateByCodeAsyncSuccess()
        {
            //Arrange
            var logger = NullLoggerFactory.Instance.CreateLogger<EmailTemplateRepository>();
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            mapper.Setup(m => m.Map<EmailTemplate,EmailTemplateModel>(It.IsAny<EmailTemplate>())).Returns(new EmailTemplateModel { });
            var unitOfWork = new Mock<IUnitOfWork>();
            var entityRepositoryMock = new Mock<IEntityRepository<EmailTemplate>>(MockBehavior.Strict);
            entityRepositoryMock.Setup(x => x.Where(It.IsAny<Specification<EmailTemplate>>())).Returns(new List<EmailTemplate>() { new EmailTemplate() }.AsQueryable().BuildMock());
            var publishJobService = new EmailTemplateRepository(logger, mapper.Object, unitOfWork.Object, entityRepositoryMock.Object);

            //Act
            var publishJobResponseModel = publishJobService.GetEmailTemplateByCodeAsync("W3H");

            //Assert
            Assert.True(publishJobResponseModel.IsCompletedSuccessfully);
        }

        [Fact]
        public void GetEmailTemplateByCodeAsyncFailed()
        {
            //Arrange
            var logger = NullLoggerFactory.Instance.CreateLogger<EmailTemplateRepository>();
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            mapper.Setup(m => m.Map<EmailTemplate, EmailTemplateModel>(It.IsAny<EmailTemplate>())).Returns(new EmailTemplateModel { });
            var unitOfWork = new Mock<IUnitOfWork>();
            var entityRepositoryMock = new Mock<IEntityRepository<EmailTemplate>>(MockBehavior.Strict);
            var publishJobService = new EmailTemplateRepository(logger, mapper.Object, unitOfWork.Object, entityRepositoryMock.Object);

            //Act
            var publishJobResponseModel = publishJobService.GetEmailTemplateByCodeAsync(null);

            //Assert
            Assert.True(publishJobResponseModel.IsFaulted);
        }
    }
}
