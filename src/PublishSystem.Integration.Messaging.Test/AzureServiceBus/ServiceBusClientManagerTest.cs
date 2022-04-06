using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PublishSystem.Core.Common.Serializations.Json;
using PublishSystem.Integration.Messaging.AzureServiceBus;
using Xunit;

namespace PublishSystem.Integration.Messaging.Test.AzureServiceBus
{
    public class ServiceBusClientManagerTest
    {
        [Fact]
        public void SendAsyncSuccess()
        {
            //Arrange
            var topic = "sbt-faketopic";
            var logger = NullLoggerFactory.Instance.CreateLogger<ServiceBusClientManager>();
            var jsonSerializer = new Mock<IJsonSerializer>(MockBehavior.Strict);
            var serviceBusClient = new Mock<ServiceBusClient>(MockBehavior.Strict);
            var sbSenderMock = new Mock<ServiceBusSender>(MockBehavior.Strict);
            sbSenderMock.Setup(x => x.EntityPath).Returns(topic);
            sbSenderMock.Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default)).Returns(Task.CompletedTask);
            serviceBusClient.Setup(x => x.CreateSender(topic)).Returns(sbSenderMock.Object);
            var sender = new Mock<Sender>(serviceBusClient.Object, topic);
            var senderList = new List<Sender>() { sender.Object };
            var @event = new FakeEvent();
            jsonSerializer.Setup(x => x.SerializeAsync(It.IsAny<IEvent>(), It.IsAny<JsonSerializerOptions>())).Returns(Task.FromResult(string.Empty));
            var sendAsync = new ServiceBusClientManager(logger, jsonSerializer.Object, senderList);

            //Act
            var response = sendAsync.SendAsync(@event, default);

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void ScheduleMessageAsyncSuccess()
        {
            //Arrange
            DateTimeOffset dateTimeOffset = new DateTimeOffset();
            var topic = "sbt-faketopic";
            var logger = NullLoggerFactory.Instance.CreateLogger<ServiceBusClientManager>();
            var jsonSerializer = new Mock<IJsonSerializer>(MockBehavior.Strict);
            var serviceBusClient = new Mock<ServiceBusClient>(MockBehavior.Strict);
            var sbSenderMock = new Mock<ServiceBusSender>(MockBehavior.Strict);
            sbSenderMock.Setup(x => x.EntityPath).Returns(topic);
            long lng = 2;
            sbSenderMock.Setup(x => x.ScheduleMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<DateTimeOffset>(), default)).Returns(Task.FromResult(lng));
            serviceBusClient.Setup(x => x.CreateSender(topic)).Returns(sbSenderMock.Object);
            var sender = new Mock<Sender>(serviceBusClient.Object, topic);
            var senderList = new List<Sender>() { sender.Object };
            var @event = new FakeEvent();
            jsonSerializer.Setup(x => x.SerializeAsync(It.IsAny<IEvent>(), It.IsAny<JsonSerializerOptions>())).Returns(Task.FromResult(string.Empty));
            var sendAsync = new ServiceBusClientManager(logger, jsonSerializer.Object, senderList);

            //Act
            var response = sendAsync.ScheduleMessageAsync(@event, dateTimeOffset, default);

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
        }

        [Fact]
        public void CancelScheduledMessageAsyncSuccess()
        {
            //Arrange
            long lng = 2;
            var topic = "sbt-faketopic";
            var logger = NullLoggerFactory.Instance.CreateLogger<ServiceBusClientManager>();
            var jsonSerializer = new Mock<IJsonSerializer>(MockBehavior.Strict);
            var serviceBusClient = new Mock<ServiceBusClient>(MockBehavior.Strict);
            var sbSenderMock = new Mock<ServiceBusSender>(MockBehavior.Strict);
            sbSenderMock.Setup(x => x.EntityPath).Returns(topic);
            sbSenderMock.Setup(x => x.CancelScheduledMessageAsync(It.IsAny<long>(), default)).Returns(Task.CompletedTask);
            serviceBusClient.Setup(x => x.CreateSender(topic)).Returns(sbSenderMock.Object);
            var sender = new Mock<Sender>(serviceBusClient.Object, topic);
            var senderList = new List<Sender>() { sender.Object };
            jsonSerializer.Setup(x => x.SerializeAsync(It.IsAny<IEvent>(), It.IsAny<JsonSerializerOptions>())).Returns(Task.FromResult(string.Empty));
            var sendAsync = new ServiceBusClientManager(logger, jsonSerializer.Object, senderList);

            //Act
            var response = sendAsync.CancelScheduledMessageAsync<FakeEvent>(lng, default);

            //Assert
            Assert.True(response.IsCompletedSuccessfully);
        }
    }
}
