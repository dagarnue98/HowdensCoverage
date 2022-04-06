namespace PublishSystem.Orchestrator.Functions.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;
    using PublishSystem.Application.Services;
    using PublishSystem.Application.Services.PublishJobService;
    using PublishSystem.Application.Services.RenderingService;
    using PublishSystem.Core.Common.Serializations.Json;
    using PublishSystem.Domain.Models;
    using PublishSystem.Domain.SeedWork;
    using Xunit;

    public class RenderingFunctionTest
    {
        [Fact]
        public void RenderingRequestFunctionSuccess()
        {
            // Arrange
            var message = new RenderingRequestModel
            {
                JobId = new Guid(),
            };
            var deliveryCount = 1;
            var enqueuedTimeUtc = DateTime.UtcNow;
            var messageId = "#001";
            var userProperties = new Dictionary<string, object>();
            var logger = NullLoggerFactory.Instance.CreateLogger<RenderingFunction>();
            var jsonSerializer = new Mock<IJsonSerializer>(MockBehavior.Strict);
            var iPublishJobService = new Mock<IPublishJobService>(MockBehavior.Strict);
            var iRenderingService = new Mock<IRenderingService>(MockBehavior.Strict);
            iRenderingService.Setup(m => m.StartRenderingAsync(It.IsAny<RenderingRequestModel>())).Returns(Task.FromResult(new LayerResponse<PublishJobModel>()));
            var renderingFunction = new RenderingFunction(logger, jsonSerializer.Object, iRenderingService.Object);

            // Act
            var response = renderingFunction.RenderingRequestFunction(message, deliveryCount, enqueuedTimeUtc, messageId, userProperties);

            // Assert
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void RenderingRequestFunctionFailed()
        {
            // Arrange
            var message = new RenderingRequestModel
            {
                JobId = new Guid(),
            };
            var deliveryCount = 1;
            var enqueuedTimeUtc = DateTime.UtcNow;
            var messageId = "#001";
            var userProperties = new Dictionary<string, object>();
            var logger = NullLoggerFactory.Instance.CreateLogger<RenderingFunction>();
            var jsonSerializer = new Mock<IJsonSerializer>(MockBehavior.Strict);
            var renderingService = new Mock<IRenderingService>(MockBehavior.Strict);
            var renderingFunction = new RenderingFunction(logger, jsonSerializer.Object, renderingService.Object);

            // Act
            var response = renderingFunction.RenderingRequestFunction(null, deliveryCount, enqueuedTimeUtc, messageId, userProperties);

            // Assert
            Assert.True(response.IsFaulted);
        }
    }
}