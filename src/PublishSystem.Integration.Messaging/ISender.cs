using Azure.Messaging.ServiceBus;

namespace PublishSystem.Integration.Messaging
{
    public interface ISender
    {
        string EntityPath { get; }
        Task SendMessagAsync(ServiceBusMessage sbMessage, CancellationToken cancellationToken = default);
        Task<long> ScheduleMessageAsync(ServiceBusMessage sbMessage, DateTimeOffset scheduledEnqueueTime, CancellationToken cancellationToken = default);
        Task CancelScheduledMessageAsync(long sequenceNumber, CancellationToken cancellationToken = default);
    }
}