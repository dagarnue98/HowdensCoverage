using Microsoft.Extensions.Logging;
using PublishSystem.Integration.Encoding.Enums;
using PublishSystem.Integration.Encoding.Models;
using PublishSystem.Integration.Encoding.Services;

namespace PublishSystem.Integration.Encoding.AzureMediaServices
{
    public class MediaServiceEncoding : IEncoding
    {
        private readonly IMediaService _mediaService;
        private readonly ILogger<MediaServiceEncoding> _logger;
        public MediaServiceEncoding(IMediaService mediaService, ILogger<MediaServiceEncoding> logger)
        {
            _mediaService = mediaService;
            _logger = logger;
        }

        /// <summary>
        /// Create an encoding job
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blobUrl"></param>
        /// <returns></returns>
        public async Task<EncodingJob> CreateEncodingJobAsync(string id, string inputAssetLocation)
        {
            _logger.LogDebug($"Creating encoding job. ID: {id}");
            var outputAssetName = await _mediaService.CreateOutputAssetAsync(id);
            var job = await _mediaService.SubmitJobAsync(outputAssetName, id, inputAssetLocation);
            _logger.LogDebug($"Submitted encoding job. ID: {id}");
            return new EncodingJob
            {
                JobName = job.Name,
                OutputAssetName = outputAssetName
            };
        }

        /// <summary>
        /// Gets the state of the encoding job
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task<JobState> GetJobStateAsync(string jobName)
        {
            var jobstate = (await _mediaService.GetJobAsync(jobName)).State;
            _logger.LogDebug($"State of {jobName} is {jobstate}");
            return (JobState)Enum.Parse(typeof(JobState), jobstate, true);
        }

        /// <summary>
        /// Gets the streaming URLs from the media service
        /// </summary>
        /// <param name="outputAssetName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Dictionary<StreamingProtocol, string>> GetStreamingUrlsAsync(string outputAssetName, string id)
        {
            var locator = await _mediaService.CreateStreamingLocatorAsync(outputAssetName, id);
            _logger.LogDebug($"Called GetStreamingUrls(). ID: {id}. OutputAssetName: {outputAssetName}. Locator: {locator.Name}");
            return await _mediaService.GetStreamingUrlsAsync(locator.Name);
        }
    }
}
