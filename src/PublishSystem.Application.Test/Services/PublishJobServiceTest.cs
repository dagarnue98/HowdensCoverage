namespace PublishSystem.Application.Test.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;
    using PublishSystem.Application.Events;
    using PublishSystem.Application.Services.PublishJobService;
    using PublishSystem.Application.Services.RenderingService;
    using PublishSystem.Domain.BusinessRules.StateManager;
    using PublishSystem.Domain.Entities;
    using PublishSystem.Domain.Enums.StateManagement;
    using PublishSystem.Domain.Models;
    using PublishSystem.Domain.Repositories.PublishJobRepository;
    using PublishSystem.Domain.SeedWork;
    using PublishSystem.Integration.Messaging;
    using Xunit;

    public class PublishJobServiceTest
    {
        [Fact]
        public void AddPublishJobAsyncSuccess()
        {
            //Arrange
            var logger = NullLoggerFactory.Instance.CreateLogger<PublishJobService>();
            var publishRepository = new Mock<IPublishJobRepository>(MockBehavior.Strict);
            var renderingService = new Mock<IRenderingService>(MockBehavior.Strict);
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            var unitOfWork = new Mock<IUnitOfWork>();
            var stateManager = new Mock<IStateManager>(MockBehavior.Strict);
            var eventBus = new Mock<IEventBus>(MockBehavior.Strict);

            var publishJobRequestModel = new PublishJobRequestModel
            {
                VersionId = 1,
                DepotNo = "",
            };

            mapper.Setup(m => m.Map<PublishJob>(It.IsAny<PublishJobRequestModel>())).Returns(new PublishJob
            {
                VersionId = 1,
                DepotNo = "",
            });

            mapper.Setup(m => m.Map<PublishJobModel, PublishJobModel>(It.IsAny<PublishJobModel>())).Returns(new PublishJobModel
            {
                VersionId = 1,
                DepotNo = "",
            });

            eventBus.Setup(m => m.SendAsync(It.IsAny<RenderingRequestEvent>(), default)).Returns(Task.CompletedTask);
            stateManager.Setup(m => m.InitialiseState(State.Requested)).Returns(State.Requested);
            publishRepository.Setup(m => m.AddPublishJobAsync(It.IsAny<PublishJobRequestModel>())).Returns(Task.FromResult(new PublishJobModel()));
            publishRepository.Setup(r => r.UpdateStateByTriggerAsync(It.IsAny<Guid>(), It.IsAny<Trigger>())).Returns(Task.FromResult(new PublishJobModel() { State = "QueuedForRendering" }));
            var publishJobService = new PublishJobService(publishRepository.Object, logger, mapper.Object, unitOfWork.Object, eventBus.Object);

            //Act
            var PublishJobModel = publishJobService.AddPublishJobAsync(publishJobRequestModel);

            //Assert
            Assert.True(PublishJobModel.IsCompletedSuccessfully);
        }

        [Fact]
        public void AddPublishJobAsync_SendEventBusError_Success()
        {
            //Arrange
            var logger = NullLoggerFactory.Instance.CreateLogger<PublishJobService>();
            var publishRepository = new Mock<IPublishJobRepository>(MockBehavior.Strict);
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            var unitOfWork = new Mock<IUnitOfWork>();
            var stateManager = new Mock<IStateManager>(MockBehavior.Strict);
            var eventBus = new Mock<IEventBus>(MockBehavior.Strict);

            var publishJobRequestModel = new PublishJobRequestModel
            {
                VersionId = 1,
                DepotNo = "",
            };

            mapper.Setup(m => m.Map<PublishJob>(It.IsAny<PublishJobRequestModel>())).Returns(new PublishJob
            {
                VersionId = 1,
                DepotNo = "",
            });

            mapper.Setup(m => m.Map<PublishJobModel>(It.IsAny<PublishJob>())).Returns(new PublishJobModel
            {
                VersionId = 1,
                DepotNo = "",
            });

            eventBus.Setup(m => m.SendAsync(It.IsAny<RenderingRequestEvent>(), default)).Returns(Task.FromException(new Exception()));
            stateManager.Setup(m => m.InitialiseState(State.Requested)).Returns(State.Requested);
            stateManager.Setup(m => m.ChangeState(Trigger.Error)).Returns(State.QueuedForRenderingError);
            publishRepository.Setup(m => m.AddPublishJobAsync(It.IsAny<PublishJobRequestModel>())).Returns(Task.FromResult(new PublishJobModel()));

            var publishJobService = new PublishJobService(publishRepository.Object, logger, mapper.Object, unitOfWork.Object, eventBus.Object);

            //Act
            var PublishJobModel = publishJobService.AddPublishJobAsync(publishJobRequestModel);

            //Assert
            Assert.False(PublishJobModel.IsCompletedSuccessfully);

        }
    }
}
