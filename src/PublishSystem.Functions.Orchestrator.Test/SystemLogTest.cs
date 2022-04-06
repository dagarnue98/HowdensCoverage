namespace PublishSystem.Orchestrator.Functions.Test
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Extensions.Logging;
    using Moq;
    using PublishSystem.Orchestrator.Functions;
    using Serilog;
    using Serilog.Sinks.TestCorrelator;
    using Xunit;

    public class SystemLogTest
    {

        [Fact]
        public void SerilogLoggingSuccess()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();

            using (TestCorrelator.CreateContext())
            {
                Log.Information("My log message!");

                Assert.Equal("My log message!", TestCorrelator.GetLogEventsFromCurrentContext().FirstOrDefault().MessageTemplate.Text);
            }
        }

        [Fact]
        public void SerilogHttpTriggerLoggingSuccess()
        {
            // Arrange
            var expectedLogMessages = 1;
            var logger = new Mock<ILogger<PublishJobFunction>>(MockBehavior.Strict);
            logger.Setup(l => l.Log(It.IsAny<LogLevel>(),
                                    It.IsAny<EventId>(),
                                    It.IsAny<It.IsAnyType>(),
                                    It.IsAny<Exception>(),
                                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
            var publishJobFunction = new PublishJobFunction(logger.Object, null);

            // Act
            var response = publishJobFunction.HttpTrigger(new DefaultHttpRequest(new DefaultHttpContext()) { });

            // Assert
            logger.Verify(
                l => l.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Exactly(expectedLogMessages));

            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void NoLoggerInjectionFailure()
        {
            // Arrange
            var publishJobFunction = new PublishJobFunction(null, null);

            // Act
            var response = publishJobFunction.HttpTrigger(new DefaultHttpRequest(new DefaultHttpContext()) { });

            // Assert
            Assert.True(response.IsFaulted);
        }
    }
}