namespace PublishSystem.Integration.Messaging
{
    public interface IEventBus
    {
        Task SendAsync(IEvent @event, CancellationToken cancellationToken = default);
        Task<long> ScheduleMessageAsync(IEvent @event, DateTimeOffset scheduledEnqueueTime, CancellationToken cancellationToken = default);
        Task CancelScheduledMessageAsync<TEvent>(long sequenceNumber, CancellationToken cancellationToken = default) where TEvent : IEvent;
    }
}
