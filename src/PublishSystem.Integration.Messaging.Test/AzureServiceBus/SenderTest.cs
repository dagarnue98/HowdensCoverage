using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Moq;
using PublishSystem.Integration.Messaging.AzureServiceBus;
using Xunit;

namespace PublishSystem.Integration.Messaging.Test.AzureServiceBus
{
    public class SenderTest
    {
        [Fact]
        public void SendMessagAsyncSuccess()
        {
            //Arrange
            var topic = "sbt-faketopic";
            var sbSenderMock = new Mock<ServiceBusSender>(MockBehavior.Strict);
            sbSenderMock.Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default)).Returns(Task.CompletedTask);
            var serviceBusClient = new Mock<ServiceBusClient>(MockBehavior.Strict);
            serviceBusClient.Setup(x => x.CreateSender(topic)).Returns(sbSenderMock.Object);
            var sender = new Sender(serviceBusClient.Object, topic);
            var serviceBusMessage = new ServiceBusMessage();

            //Act
            var response = sender.SendMessagAsync(serviceBusMessage, default);

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void ScheduleMessageAsyncSuccess()
        {
            //Arrange
            var topic = "sbt-faketopic";
            DateTimeOffset dateTimeOffset = new ();
            var sbSenderMock = new Mock<ServiceBusSender>(MockBehavior.Strict);
            long lng = 2;
            sbSenderMock.Setup(x => x.ScheduleMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<DateTimeOffset>(), default)).Returns(Task.FromResult(lng));
            var serviceBusClient = new Mock<ServiceBusClient>(MockBehavior.Strict);
            serviceBusClient.Setup(x => x.CreateSender(topic)).Returns(sbSenderMock.Object);
            var sender = new Sender(serviceBusClient.Object, topic);
            var serviceBusMessage = new ServiceBusMessage();

            //Act
            var response = sender.ScheduleMessageAsync(serviceBusMessage, dateTimeOffset, default);

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void CancelScheduledMessageAsyncSuccess()
        {
            //Arrange
            var topic = "sbt-faketopic";
            long lng = 2;
            var sbSenderMock = new Mock<ServiceBusSender>(MockBehavior.Strict);
            sbSenderMock.Setup(x => x.CancelScheduledMessageAsync(It.IsAny<long>(), default)).Returns(Task.CompletedTask);
            var serviceBusClient = new Mock<ServiceBusClient>(MockBehavior.Strict);
            serviceBusClient.Setup(x => x.CreateSender(topic)).Returns(sbSenderMock.Object);
            var sender = new Sender(serviceBusClient.Object, topic);
            var serviceBusMessage = new ServiceBusMessage();

            //Act
            var response = sender.CancelScheduledMessageAsync(lng, default);

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
        }
    }
}
