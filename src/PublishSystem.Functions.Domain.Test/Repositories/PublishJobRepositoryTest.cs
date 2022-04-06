using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MockQueryable.Moq;
using Moq;
using PublishSystem.Domain.BusinessRules.StateManager;
using PublishSystem.Domain.Entities;
using PublishSystem.Domain.Enums.StateManagement;
using PublishSystem.Domain.Models;
using PublishSystem.Domain.Repositories.PublishJobRepository;
using PublishSystem.Domain.SeedWork;
using Xunit;

namespace PublishSystem.Domain.Test.Repositories
{
    public class PublishJobRepositoryTest
    {
        [Fact]
        public void AddPublishJobAsyncSuccess()
        {
            //Arrange
            var logger = NullLoggerFactory.Instance.CreateLogger<PublishJobRepository>();
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            var unitOfWork = new Mock<IUnitOfWork>();
            var stateManager = new Mock<IStateManager>(MockBehavior.Strict);
            var entityRepository = new Mock<IEntityRepository<PublishJob>>(MockBehavior.Strict);

            var publishJobRequestModel = new PublishJobRequestModel
            {
                VersionId = 1,
                DepotNo = string.Empty,
            };

            mapper.Setup(m => m.Map<PublishJob>(It.IsAny<PublishJobRequestModel>())).Returns(new PublishJob
            {
                VersionId = 1,
                DepotNo = string.Empty,
            });

            mapper.Setup(m => m.Map<PublishJob, PublishJobModel>(It.IsAny<PublishJob>())).Returns(new PublishJobModel
            {
                VersionId = 1,
                DepotNo = string.Empty,
            });

            stateManager.Setup(m => m.InitialiseState(State.Requested)).Returns(State.Requested);
            entityRepository.Setup(m => m.Add(It.IsAny<PublishJob>())).Returns(new PublishJob());
            var publishJobService = new PublishJobRepository(logger, mapper.Object, unitOfWork.Object, entityRepository.Object, stateManager.Object);

            //Act
            var publishJobResponseModel = publishJobService.AddPublishJobAsync(publishJobRequestModel);

            //Assert
            Assert.True(publishJobResponseModel.IsCompletedSuccessfully);
        }

        [Fact]
        public void UpdateStateToErrorAsyncSuccess()
        {
            //Arrange
            var logger = NullLoggerFactory.Instance.CreateLogger<PublishJobRepository>();
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            var unitOfWork = new Mock<IUnitOfWork>();
            var stateManager = new Mock<IStateManager>(MockBehavior.Strict);
            var entityRepository = new Mock<IEntityRepository<PublishJob>>(MockBehavior.Strict);

            mapper.Setup(m => m.Map<PublishJob, PublishJobModel>(It.IsAny<PublishJob>())).Returns(new PublishJobModel
            {
                VersionId = 1,
                DepotNo = string.Empty,
            });

            stateManager.Setup(m => m.InitialiseState(State.Requested)).Returns(State.Requested);
            stateManager.Setup(m => m.ChangeState(Trigger.Error)).Returns(State.QueuedForRenderingError);

            entityRepository.Setup(x => x.Where(It.IsAny<Specification<PublishJob>>())).Returns(new List<PublishJob>() { new PublishJob() { } }.AsQueryable().BuildMock());

            var publishJobService = new PublishJobRepository(logger, mapper.Object, unitOfWork.Object, entityRepository.Object, stateManager.Object);

            //Act
            var publishJobResponseModel = publishJobService.UpdateStateToErrorAsync(Guid.NewGuid());

            //Assert
            Assert.True(publishJobResponseModel.IsCompletedSuccessfully);
        }

        [Fact]
        public void GetPublishJobByBatchJobIdSucced()
        {
            //Arrange
            var entityRepositoryMock = new Mock<IEntityRepository<PublishJob>>();
            entityRepositoryMock.Setup(x => x.Where(It.IsAny<Specification<PublishJob>>())).Returns(new List<PublishJob>() { new PublishJob() }.AsQueryable().BuildMock());
            var logger = new Mock<ILogger<PublishJobRepository>>(MockBehavior.Strict);
            logger.Setup(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()));

            var mapper = new Mock<IMapper>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var stateManagementService = new Mock<IStateManager>();

            var publishJobRepository = new PublishJobRepository(logger.Object, mapper.Object, unitOfWork.Object, entityRepositoryMock.Object, stateManagementService.Object);

            //Act
            var response = publishJobRepository.GetPublishJobByBatchJobId("4533-233ddf-232df");

            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void GetPublishJobByJobIdSuccess()
        {
            //Arrange

            var entityRepositoryMock = new Mock<IEntityRepository<PublishJob>>();
            entityRepositoryMock.Setup(x => x.Where(It.IsAny<Specification<PublishJob>>())).Returns(new List<PublishJob>() { new PublishJob() }.AsQueryable().BuildMock());
            var logger = new Mock<ILogger<PublishJobRepository>>(MockBehavior.Strict);
            logger.Setup(l => l.Log(It.IsAny<LogLevel>(),
                                    It.IsAny<EventId>(),
                                    It.IsAny<It.IsAnyType>(),
                                    It.IsAny<Exception>(),
                                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()));

            var mapper = new Mock<IMapper>();

            var unitOfWork = new Mock<IUnitOfWork>();
            var stateManagementService = new Mock<IStateManager>();

            var publishJobRepository = new PublishJobRepository(logger.Object, mapper.Object, unitOfWork.Object, entityRepositoryMock.Object, stateManagementService.Object);

            //Act
            var response = publishJobRepository.GetPublishJobByJobIdAsync(new Guid());

            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsCompletedSuccessfully);
        }
    }
}
