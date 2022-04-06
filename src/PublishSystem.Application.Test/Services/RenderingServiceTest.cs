namespace PublishSystem.Application.Test.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Moq;
    using PublishSystem.Application.Services.RenderingService;
    using PublishSystem.Domain.BusinessRules.StateManager;
    using PublishSystem.Domain.Entities;
    using PublishSystem.Domain.Enums.StateManagement;
    using PublishSystem.Domain.Models;
    using PublishSystem.Domain.Repositories.PublishJobRepository;
    using PublishSystem.Domain.SeedWork;
    using PublishSystem.Integration.Messaging;
    using PublishSystem.Integration.Rendering.AzureBatch.Events;
    using Xunit;

    public class RenderingServiceTest
    {
        [Fact]
        public void StartRenderingAsyncSuccess()
        {
            //Arrange
            var publishJobRepository = new Mock<IPublishJobRepository>(MockBehavior.Strict);
            publishJobRepository.Setup(r => r.UpdateStateByTriggerAsync(It.IsAny<Guid>(), It.IsAny<Trigger>())).Returns(Task.FromResult(new PublishJobModel() { State = "Rendering" }));
            var logger = new Mock<ILogger<RenderingService>>(MockBehavior.Strict);
            logger.Setup(l =>
                l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()));
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<PublishJob, PublishJobModel>(It.IsAny<PublishJob>())).Returns(new PublishJobModel());
            var unitOfWork = new Mock<IUnitOfWork>();
            var eventBus = new Mock<IEventBus>(MockBehavior.Strict);
            var stateManagementService = new Mock<IStateManager>();

            var renderingService = new RenderingService(publishJobRepository.Object, logger.Object, mapper.Object, unitOfWork.Object, eventBus.Object);

            var renderingRequestModel = new RenderingRequestModel() { JobId = Guid.NewGuid() };

            //Act
            var response = renderingService.StartRenderingAsync(renderingRequestModel);

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void StartRenderingAsyncError()
        {
            //Arrange
            var publishJobRepository = new Mock<IPublishJobRepository>(MockBehavior.Strict);
            var logger = new Mock<ILogger<RenderingService>>(MockBehavior.Strict);
            logger.Setup(l => l.Log(
                                It.IsAny<LogLevel>(),
                                It.IsAny<EventId>(),
                                It.IsAny<It.IsAnyType>(),
                                It.IsAny<Exception>(),
                                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()));
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<PublishJob, PublishJobModel>(It.IsAny<PublishJob>())).Returns(new PublishJobModel());
            var unitOfWork = new Mock<IUnitOfWork>();
            var eventBus = new Mock<IEventBus>(MockBehavior.Strict);
            var stateManagementService = new Mock<IStateManager>();

            var renderingService = new RenderingService(publishJobRepository.Object, logger.Object, mapper.Object, unitOfWork.Object, eventBus.Object);

            //Act
            var response = renderingService.StartRenderingAsync(null!);

            //Assert
            Assert.True(response.IsFaulted);
        }

        [Fact]
        public void HandlerRenderingEventSuccess()
        {
            //Arrange
            var publishJobRepository = new Mock<IPublishJobRepository>(MockBehavior.Strict);
            publishJobRepository.Setup(r => r.GetPublishJobByBatchJobId(It.IsAny<string>())).Returns(Task.FromResult(new PublishJobModel() { State = "Rendering" }));
            publishJobRepository.Setup(r => r.UpdateStateByTriggerAsync(It.IsAny<string>(), It.IsAny<Trigger>())).Returns(Task.FromResult(new PublishJobModel() { State = "Rendered" }));
            var logger = new Mock<ILogger<RenderingService>>(MockBehavior.Strict);
            logger.Setup(l => l.Log(It.IsAny<LogLevel>(),
                                    It.IsAny<EventId>(),
                                    It.IsAny<It.IsAnyType>(),
                                    It.IsAny<Exception>(),
                                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
            var mapper = new Mock<IMapper>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var eventBus = new Mock<IEventBus>(MockBehavior.Strict);
            var stateManagementService = new Mock<IStateManager>();
            var renderingService = new RenderingService(publishJobRepository.Object, logger.Object, mapper.Object,
                  unitOfWork.Object, eventBus.Object);

            var encodeRequestModel = new TaskEvent() { JobId = "test", ExecutionInfo = new ExecutionInfo() { ExitCode = 0 } };

            //Act
            var response = renderingService.HandlerRenderingEventAsync(encodeRequestModel);

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
            Assert.Equal("Rendered", response.Result.Content.State);
        }

        [Fact]
        public void HandlerRenderingEventError()
        {
            //Arrange
            var publishJobRepository = new Mock<IPublishJobRepository>(MockBehavior.Strict);
            publishJobRepository.Setup(r => r.GetPublishJobByJobIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new LayerResponse<PublishJobModel>(new PublishJobModel() { State = "Requested" })));
            var logger = new Mock<ILogger<RenderingService>>(MockBehavior.Strict);
            logger.Setup(l => l.Log(It.IsAny<LogLevel>(),
                                    It.IsAny<EventId>(),
                                    It.IsAny<It.IsAnyType>(),
                                    It.IsAny<Exception>(),
                                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<PublishJob, PublishJobModel>(It.IsAny<PublishJob>())).Returns(new PublishJobModel());
            var unitOfWork = new Mock<IUnitOfWork>();
            var eventBus = new Mock<IEventBus>(MockBehavior.Strict);
            var stateManagementService = new Mock<IStateManager>();

            var renderingService = new RenderingService(publishJobRepository.Object, logger.Object, mapper.Object,
                  unitOfWork.Object, eventBus.Object);


            //Act
            var response = renderingService.HandlerRenderingEventAsync(null);

            //Assert
            Assert.True(response.IsFaulted);
        }
    }
}
