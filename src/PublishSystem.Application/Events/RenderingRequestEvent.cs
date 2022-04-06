using PublishSystem.Domain.Models;
using PublishSystem.Integration.Messaging;
using PublishSystem.Integration.Messaging.Extensions;

namespace PublishSystem.Application.Events
{
    [EntityPath("manualrendering")]
    public class RenderingRequestEvent : RenderingRequestModel, IEvent
    {
    }
}
