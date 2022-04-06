using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PublishSystem.Application.Services.PublishJobService;
using PublishSystem.Application.Services.RenderingService;
using PublishSystem.Core.Common.Serializations.Json;
using PublishSystem.Domain.Models;
using PublishSystem.Domain.SeedWork;
using Xunit;

namespace PublishSystem.Orchestrator.Functions.Test
{
    public class PublishJobFunctionTest
    {
        private const string _versionId = "0123456789";
        private Guid _guid = Guid.NewGuid();
        private readonly ILogger<PublishJobFunction> _logger;
        private readonly Mock<IPublishJobService> _publishJobService;
        private readonly PublishJobFunction _systemUnderTest;

        public PublishJobFunctionTest()
        {
            _logger = NullLoggerFactory.Instance.CreateLogger<PublishJobFunction>();
            _publishJobService = new Mock<IPublishJobService>(MockBehavior.Strict);

            _systemUnderTest = new PublishJobFunction(_logger, _publishJobService.Object);
        }

        [Fact]
        public void RunHttpTriggerSuccess()
        {
            // Arrange
            var request = new DefaultHttpRequest(new DefaultHttpContext()) { };

            //Act
            var response = _systemUnderTest.HttpTrigger(request);

            var result = (ObjectResult)response.Result;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void RunServiceBusTriggerSuccess()
        {
            // Arrange
            var message = new PublishJobRequestModel
            {
                DepotNo = "DepotNo1",
                Name = "Name",
            };
            var deliveryCount = 1;
            var enqueuedTimeUtc = DateTime.UtcNow;
            var messageId = "#001";
            var userProperties = new Dictionary<string, object>();

            _publishJobService.Setup(m => m.AddPublishJobAsync(It.IsAny<PublishJobRequestModel>())).Returns(Task.FromResult(new LayerResponse<PublishJobModel>()));

            //Act
            var response = _systemUnderTest.Run(message, deliveryCount, enqueuedTimeUtc, messageId, userProperties);

            // Assert
            Assert.True(response.IsCompletedSuccessfully);

        }

        [Fact]
        public void RenderingRequestFunctionSuccess()
        {
            //Arrange
            var message = new RenderingRequestModel
            {
                JobId = new Guid()
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

            //Act
            var response = renderingFunction.RenderingRequestFunction(message, deliveryCount, enqueuedTimeUtc, messageId, userProperties);

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task GetPublishJobByVersionId_ContentPopulated_ReturnSuccess()
        {
            //Arrange
            _publishJobService.Setup(p => p.GetPublishJobByVersionIdAsync(Int32.Parse(_versionId)))
                             .ReturnsAsync(new LayerResponse<PublishJobModel>() { Content = new PublishJobModel()});

            var request = new Mock<HttpRequest>();

            //Act
            var response = await _systemUnderTest.GetPublishJobByVersionId(request.Object, _versionId);
            var result = response as OkObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task GetPublishJobByVersionId_HasError_ReturnBadRequest()
        {
            //Arrange
            _publishJobService.Setup(p => p.GetPublishJobByVersionIdAsync(Int32.Parse(_versionId)))
                             .ReturnsAsync(new LayerResponse<PublishJobModel>("Error"));

            var request = new Mock<HttpRequest>();

            //Act
            var response = await _systemUnderTest.GetPublishJobByVersionId(request.Object, _versionId);
            var result = response as BadRequestObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GetPublishJobByVersionId_InvalidQueryValue_ReturnBadRequest()
        {
            //Arrange
            var request = new Mock<HttpRequest>();

            //Act
            var response = await _systemUnderTest.GetPublishJobByVersionId(request.Object, "This should fail");
            var result = response as BadRequestObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GetPublishStateDiagram_ContentPopulated_ReturnSuccess()
        {
            //Arrange
            _publishJobService.Setup(p => p.GetPublishStateDiagram())
                             .Returns(new LayerResponse<string>() { Content = "This should succeed" });

            var request = new Mock<HttpRequest>();

            //Act
            var response = _systemUnderTest.GetPublishStateDiagram(request.Object);
            var result = response as OkObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task GetPublishJobByJobId_ContentPopulated_ReturnSuccess()
        {
            //Arrange
            _publishJobService.Setup(p => p.GetPublishJobByJobIdAsync(_guid))
                             .ReturnsAsync(new LayerResponse<PublishJobModel>() { Content = new PublishJobModel() });

            var request = new Mock<HttpRequest>();

            //Act
            var response = await _systemUnderTest.GetPublishJobByJobId(request.Object, _guid);
            var result = response as OkObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task GetPublishJobByJobId_HasError_ReturnBadRequest()
        {
            //Arrange
            _publishJobService.Setup(p => p.GetPublishJobByJobIdAsync(_guid))
                             .ReturnsAsync(new LayerResponse<PublishJobModel>("Error"));

            var request = new Mock<HttpRequest>();

            //Act
            var response = await _systemUnderTest.GetPublishJobByJobId(request.Object, _guid);
            var result = response as BadRequestObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}