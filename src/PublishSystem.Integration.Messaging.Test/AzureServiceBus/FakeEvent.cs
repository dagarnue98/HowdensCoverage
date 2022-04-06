using PublishSystem.Integration.Messaging.Extensions;

namespace PublishSystem.Integration.Messaging.Test.AzureServiceBus
{
    [EntityPath("sbt-faketopic")]
    public class FakeEvent : IEvent
    {
    }
}
