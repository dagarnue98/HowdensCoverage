namespace PublishSystem.Orchestrator.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Azure.Messaging.EventHubs;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using PublishSystem.Application.Services.RenderingService;
    using PublishSystem.Core.Common.Serializations.Json;
    using PublishSystem.Domain.Models;
    using PublishSystem.Integration.Rendering.AzureBatch.Events;

    public class RenderingFunction
    {
        private readonly ILogger<RenderingFunction> _logger;
        private readonly IRenderingService _renderingService;
        private readonly IJsonSerializer _jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingFunction"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="renderingService">Rendering Service Interface.</param>
        public RenderingFunction(ILogger<RenderingFunction> logger, IJsonSerializer jsonSerializer, IRenderingService renderingService)
        {
            _logger = logger;
            _jsonSerializer = jsonSerializer;
            _renderingService = renderingService;
        }

        [FunctionName("RenderingRequestFunction")]
        public async Task RenderingRequestFunction(
                [ServiceBusTrigger(topicName: "manualrendering", subscriptionName: "manualrenderingsub", Connection = "AzureServiceBus:ConnectionString")]
                RenderingRequestModel message,
                int deliveryCount,
                DateTime enqueuedTimeUtc,
                string messageId,
                IDictionary<string, object> applicationProperties)
        {
            try
            {
                _ = await _renderingService.StartRenderingAsync(message);

                _logger.LogInformation($"C# ServiceBus topic trigger function Processed message: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"C# ServiceBus topic trigger function. Failed message: {message}. Exception: {ex}");
                throw;
            }
        }

        [FunctionName("EventHubTrigger")]
        [FixedDelayRetry(6, "00:00:20")]
        public async Task EventHubTrigger(
                    [EventHubTrigger("insights-logs-servicelog", Connection = "EventHubConnectionString", ConsumerGroup = "$Default")]
                    EventData myEventHubMessage,
                    DateTime enqueuedTimeUtc,
                    long sequenceNumber,
                    string offset,
                    ILogger log)
        {
            var eventBody = Encoding.UTF8.GetString(myEventHubMessage.EventBody.ToArray());
            _logger.LogInformation($"Event: {eventBody}");

            var taskEvent = JsonSerializer.Deserialize<TaskEvent>(myEventHubMessage.EventBody);
            _ = await _renderingService.HandlerRenderingEventAsync(taskEvent);

            _logger.LogDebug($"BodyId : {taskEvent.JobId}");
        }
    }
}
