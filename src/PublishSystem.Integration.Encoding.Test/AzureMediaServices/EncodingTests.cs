using Microsoft.Azure.Management.Media.Models;
using Microsoft.Extensions.Logging;
using Moq;
using PublishSystem.Integration.Encoding.AzureMediaServices;
using PublishSystem.Integration.Encoding.Enums;
using PublishSystem.Integration.Encoding.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PublishSystem.Integration.Encoding.Test.AzureMediaServices
{
    public class EncodingTests
    {
        Mock<IMediaService> _mediaServiceMock;
        MediaServiceEncoding _sut;
        string _id = "4737BF87-655A-4F6D-A2D2-8C7B9CFCDDD2";
        string _blobUrl = "http://BlobUrl";
        string _outputAssetName = "outputAssetName";
        string _jobName = "JobName";

        public EncodingTests()
        {
            _mediaServiceMock = new Mock<IMediaService>();
            var _logger = new Mock<ILogger<MediaServiceEncoding>>(MockBehavior.Strict);
            _logger.Setup(l => l.Log(It.IsAny<LogLevel>(),
                                    It.IsAny<EventId>(),
                                    It.IsAny<It.IsAnyType>(),
                                    It.IsAny<Exception>(),
                                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
            _sut = new MediaServiceEncoding(_mediaServiceMock.Object, _logger.Object);
        }

        [Fact]
        public async Task CreateEncodingJob_ShouldReturnEncodingJobResponseModel()
        {
            //arrange
            var job = new Job(null, null, null, _jobName);
            _mediaServiceMock.Setup(s => s.CreateOutputAssetAsync(_id)).ReturnsAsync(_outputAssetName);
            _mediaServiceMock.Setup(s => s.SubmitJobAsync(_outputAssetName, _id, _blobUrl)).ReturnsAsync(job);
            //act
            var response = await _sut.CreateEncodingJobAsync(_id, _blobUrl);

            //assert
            Assert.Equal(_jobName, response.JobName);
            Assert.Equal(_outputAssetName, response.OutputAssetName);
        }

        [Theory]
        [InlineData("Canceled")]
        [InlineData("Canceling")]
        [InlineData("Error")]
        [InlineData("Finished")]
        [InlineData("Processing")]
        [InlineData("Queued")]
        [InlineData("Scheduled")]
        public async Task GetJobState_ShouldReturnState(string state)
        {
            //arrange
            var job = new Job(null, null, null, _jobName, null, default(DateTime), state);
            _mediaServiceMock.Setup(s => s.GetJobAsync(_jobName)).ReturnsAsync(job);

            //act
            var response = await _sut.GetJobStateAsync(_jobName);

            //assert
            var expectedStatus = (Enums.JobState)Enum.Parse(typeof(Enums.JobState), state, true);
            Assert.Equal(expectedStatus, response);
        }

        [Fact]
        public async Task GetStreamingUrls_ShouldReturnUrls()
        {
            //arrange
            var locatorName = "locatorName";
            var streamingLocator = new StreamingLocator(null, null, null, locatorName);
            var urls = new Dictionary<StreamingProtocol, string> {
                {StreamingProtocol.Hls, "https://url1" },
                {StreamingProtocol.Dash, "https://url2" },
                {StreamingProtocol.SmoothStreaming, "https://url3" }
            };
            _mediaServiceMock.Setup(s => s.CreateStreamingLocatorAsync(_outputAssetName, _id)).ReturnsAsync(streamingLocator);
            _mediaServiceMock.Setup(s => s.GetStreamingUrlsAsync(locatorName)).ReturnsAsync(urls);

            //act
            var response = await _sut.GetStreamingUrlsAsync(_outputAssetName, _id);

            //assert
            Assert.Equal(urls.Count, response.Count);
            foreach (var url in urls)
            {
                var actual = response[url.Key];
                Assert.Equal(url.Value, actual);
            }
        }
    }
}
