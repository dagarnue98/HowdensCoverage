using Azure.Messaging.ServiceBus;

namespace PublishSystem.Integration.Messaging.AzureServiceBus
{
    public class Sender : ISender
    {
        private readonly ServiceBusSender _sender;

        public Sender(ServiceBusClient serviceBusClient, string queueOrTopicName)
        {
            _sender = serviceBusClient.CreateSender(queueOrTopicName);
        }

        public string EntityPath
        {
            get { return _sender.EntityPath; }
        }

        public async Task SendMessagAsync(ServiceBusMessage sbMessage, CancellationToken cancellationToken = default)
        {
            await _sender.SendMessageAsync(sbMessage, cancellationToken);
        }

        public async Task<long> ScheduleMessageAsync(ServiceBusMessage sbMessage, DateTimeOffset scheduledEnqueueTime, CancellationToken cancellationToken = default)
        {
            var sequenceNumber = await _sender.ScheduleMessageAsync(sbMessage, scheduledEnqueueTime, cancellationToken);
            return sequenceNumber;
        }

        public async Task CancelScheduledMessageAsync(long sequenceNumber, CancellationToken cancellationToken = default)
        {
            await _sender.CancelScheduledMessageAsync(sequenceNumber, cancellationToken);
        }
    }
}
