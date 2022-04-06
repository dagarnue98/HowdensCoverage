using System;
using Microsoft.Extensions.Logging;
using Moq;
using PublishSystem.Domain.Enums.StateManagement;
using Xunit;

namespace PublishSystem.Domain.Test.StateManager
{
    public class StateManagerTest
    {
        [Theory]
        [InlineData(State.Requested, Trigger.Send, State.QueuedForRendering)]
        [InlineData(State.QueuedForRendering, Trigger.Subscribe, State.Rendering)]
        [InlineData(State.Rendering, Trigger.Completed, State.Rendered)]
        [InlineData(State.Rendered, Trigger.Send, State.QueuedForEncoding)]
        [InlineData(State.QueuedForEncoding, Trigger.Subscribe, State.Encoding)]
        [InlineData(State.Encoding, Trigger.Completed, State.Encoded)]
        [InlineData(State.Encoded, Trigger.Send, State.Published)]
        [InlineData(State.Published, Trigger.Completed, State.EmailSent)]
        [InlineData(State.Requested, Trigger.Error, State.QueuedForRenderingError)]
        [InlineData(State.QueuedForRendering, Trigger.Error, State.SubscribedToRenderingError)]
        [InlineData(State.Rendering, Trigger.Error, State.RenderingError)]
        [InlineData(State.Rendered, Trigger.Error, State.QueuedForEncodingError)]
        [InlineData(State.QueuedForEncoding, Trigger.Error, State.SubscribedToEncodingError)]
        [InlineData(State.Encoding, Trigger.Error, State.EncodingError)]
        [InlineData(State.Encoded, Trigger.Error, State.PublishedError)]
        [InlineData(State.Published, Trigger.Error, State.EmailError)]
        public void Transition(State initialState, Trigger trigger, State expectedState)
        {
            // Arrange
            var logger = new Mock<ILogger<BusinessRules.StateManager.StateManager>>(MockBehavior.Strict);
            logger.Setup(l => l.Log(
                                It.IsAny<LogLevel>(),
                                It.IsAny<EventId>(),
                                It.IsAny<It.IsAnyType>(),
                                It.IsAny<Exception>(),
                                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()));

            var stateManager = new BusinessRules.StateManager.StateManager(logger.Object);
            stateManager.InitialiseState(initialState);

            // Act
            var state = stateManager.ChangeState(trigger);

            // Assert
            Assert.Equal(expectedState, state);
        }
    }
}
