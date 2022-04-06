using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using PublishSystem.Core.Common.Serializations.Json;
using PublishSystem.Integration.Messaging.Extensions;

namespace PublishSystem.Integration.Messaging.AzureServiceBus
{
    public class ServiceBusClientManager : IEventBus
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger<ServiceBusClientManager> _logger;
        private readonly IEnumerable<ISender> _senders;

        public ServiceBusClientManager(ILogger<ServiceBusClientManager> logger, IJsonSerializer jsonSerializer, IEnumerable<ISender> senders)
        {
            _logger = logger;
            _jsonSerializer = jsonSerializer;
            _senders = senders;
        }

        public async Task SendAsync(IEvent @event, CancellationToken cancellationToken = default)
        {
            var sender = GetSender(@event);
            var sbMessage = await CreateServiceBusMessageAsync(@event);
            await sender.SendMessagAsync(sbMessage, cancellationToken);
        }

        public async Task<long> ScheduleMessageAsync(IEvent @event, DateTimeOffset scheduledEnqueueTime, CancellationToken cancellationToken = default)
        {
            var sbMessage = await CreateServiceBusMessageAsync(@event);
            var sender = GetSender(@event);
            return await sender. ScheduleMessageAsync(sbMessage, scheduledEnqueueTime, cancellationToken);
        }

        public async Task CancelScheduledMessageAsync<TEvent>(long sequenceNumber, CancellationToken cancellationToken = default) where TEvent : IEvent
        {
            var entityPath = EventExtensions.GetEntityPath<TEvent>(_logger);
            var sender = GetSender(entityPath);
            await sender.CancelScheduledMessageAsync(sequenceNumber, cancellationToken);
        }

        private ISender GetSender(IEvent @event)
        {
            var entityPath = @event.GetEntityPath(_logger);
            var sender = GetSender(entityPath);
            return sender;
        }

        private ISender GetSender(string entityPath)
        {
            var sender = _senders.Single(s => s.EntityPath == entityPath);
            return sender;
        }

        private async Task<ServiceBusMessage> CreateServiceBusMessageAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var settings = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var message = await _jsonSerializer.SerializeAsync(@event, settings);
            var sbMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message))
            {
                ContentType = ContentTypes.JSON,
                Subject = @event.GetType().Name
            };
            sbMessage.ApplicationProperties.Add("EventType", @event.GetType().Name);
            return sbMessage;
        }
    }
}
