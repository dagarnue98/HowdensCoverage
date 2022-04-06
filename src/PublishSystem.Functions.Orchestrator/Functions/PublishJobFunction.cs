namespace PublishSystem.Orchestrator.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PublishSystem.Application.Services.PublishJobService;
    using PublishSystem.Domain.Extensions;
    using PublishSystem.Domain.Models;


    /// <summary>
    /// Publish Job Function class.
    /// </summary>
    public class PublishJobFunction
    {
        private readonly ILogger<PublishJobFunction> _logger;
        private readonly IPublishJobService _publishJobService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishJobFunction"/> class.
        /// </summary>
        /// <param name="log">Log.</param>
        /// <param name="publishJobService">Publish job Service Interface.</param>
        public PublishJobFunction(ILogger<PublishJobFunction> log, IPublishJobService publishJobService)
        {
            _logger = log;
            _publishJobService = publishJobService;
        }

        [FunctionName("PublishRequestFunction")]
        public async Task Run(
                [ServiceBusTrigger(topicName: "manualrequest", subscriptionName: "manualrequestsub", Connection = "AzureServiceBus:ConnectionString")]
                PublishJobRequestModel message,
                int deliveryCount,
                DateTime enqueuedTimeUtc,
                string messageId,
                IDictionary<string, object> applicationProperties)
        {
            try
            {
                var response = await _publishJobService.AddPublishJobAsync(message);

                _logger.LogInformation($"C# ServiceBus topic trigger function Processed message: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"C# ServiceBus topic trigger function. Failed message: {message}. Exception: {ex}");
                throw;
            }
        }

        [FunctionName("HttpTrigger")]
        public async Task<IActionResult> HttpTrigger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogDebug("C# HTTP trigger function processed a request.");
            string responseMessage = "200 OK. It is alive";

            return new OkObjectResult(responseMessage);
        }

        /// <summary>
        /// Gets a publish job by version ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [FunctionName("GetPublishJobByVersionId")]
        public async Task<IActionResult> GetPublishJobByVersionId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "publish/version/{id}")] HttpRequest request,
        string id)
        {
            if (!int.TryParse(id, out var versionId))
            {
                _logger.LogDebug($"GetPublishJobByVersionId() called with invalid value for query parameter");
                return new BadRequestObjectResult($"Invalid value for query parameter version ID'");
            }
            _logger.LogInformation($"GetPublishJobByVersionId() called with version ID {versionId}");

            var serviceResponse = await _publishJobService.GetPublishJobByVersionIdAsync(versionId);
            _logger.LogInformation($"Call to GetPublishJobByVersionId() with version ID {versionId} completed");

            return serviceResponse.GetHttpResponse();
        }

        /// <summary>
        /// Gets a publish job by job ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [FunctionName("GetPublishJobByJobId")]
        public async Task<IActionResult> GetPublishJobByJobId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "publish/job/{id}")] HttpRequest request,
        Guid id)
        {
            _logger.LogInformation($"GetPublishJobByJobId() called with job ID {id}");

            var serviceResponse = await _publishJobService.GetPublishJobByJobIdAsync(id);
            _logger.LogInformation($"Call to GetPublishJobByJobId() with job ID {id} completed");

            return serviceResponse.GetHttpResponse();
        }

        [FunctionName("GetPublishStateDiagram")]
        public IActionResult GetPublishStateDiagram(
                    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request)
        {
            _logger.LogInformation("GetPublishStateDiagram() called");

            var serviceResponse = _publishJobService.GetPublishStateDiagram();
            _logger.LogInformation("GetPublishStateDiagram() call completed");

            return serviceResponse.GetHttpResponse();
        }
    }
}
