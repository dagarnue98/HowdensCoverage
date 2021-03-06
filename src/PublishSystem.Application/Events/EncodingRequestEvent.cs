using PublishSystem.Domain.Models;
using PublishSystem.Integration.Messaging;
using PublishSystem.Integration.Messaging.Extensions;

namespace PublishSystem.Application.Events
{
    [EntityPath("sbt-encodingrequest")]
    public class EncodingRequestEvent : RenderingRequestModel, IEvent
    {
    }
}
