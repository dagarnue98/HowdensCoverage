using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PublishSystem.Integration.Encoding.Enums;
using PublishSystem.Integration.Encoding.Options;

namespace PublishSystem.Integration.Encoding.Services
{
    public class MediaService : IMediaService
    {
        private readonly IAzureMediaServicesClient _client;
        private readonly ILogger<MediaService> _logger;
        private readonly MediaServicesOptions _mediaServicesOptions;
        private const string _uriScheme = "https";
        public MediaService(AzureMediaServicesClient azureMediaServicesClient, IOptions<MediaServicesOptions> mediaServicesOptions, ILogger<MediaService> logger)
        {
            _mediaServicesOptions = mediaServicesOptions.Value;
            _client = azureMediaServicesClient;
            _logger = logger;
        }

        /// <summary>
        /// Create an Azure Media Service output asset 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CreateOutputAssetAsync(string id)
        {
            string assetName = $"Output-{id}";
            _logger.LogDebug($"Called CreateOutputAsset(): {assetName}");
            return (await _client.Assets.CreateOrUpdateAsync(_mediaServicesOptions.ResourceGroupName, _mediaServicesOptions.MediaServicesAccountName, assetName, new Asset())).Name;
        }

        /// <summary>
        /// Submits an Azure Media Service job
        /// </summary>
        /// <param name="outputAssetName"></param>
        /// <param name="id"></param>
        /// <param name="blobLocationUrl"></param>
        /// <returns></returns>
        public async Task<Job> SubmitJobAsync(string outputAssetName, string id, string blobLocationUrl)
        {
            var jobName = $"job-{id}";
            _logger.LogDebug($"Called SubmitJob(): {jobName}");
            JobInput jobInput = new JobInputHttp(files: new[] { blobLocationUrl });

            JobOutput[] jobOutputs =
            {
                new JobOutputAsset(outputAssetName),
            };

            return await _client.Jobs.CreateAsync(
                _mediaServicesOptions.ResourceGroupName,
                _mediaServicesOptions.MediaServicesAccountName,
                _mediaServicesOptions.TransformName,
                jobName,
                new Job
                {
                    Input = jobInput,
                    Outputs = jobOutputs,
                });
        }

        /// <summary>
        /// Gets an Azure Media Service job
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task<Job> GetJobAsync(string jobName)
        {
            _logger.LogDebug($"Called GetJob(): {jobName}");
            return await _client.Jobs.GetAsync(_mediaServicesOptions.ResourceGroupName, _mediaServicesOptions.MediaServicesAccountName, _mediaServicesOptions.TransformName, jobName);
        }

        /// <summary>
        /// Creates a streaming locator
        /// </summary>
        /// <param name="outputAssetName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StreamingLocator> CreateStreamingLocatorAsync(string outputAssetName, string id)
        {
            _logger.LogDebug($"Called CreateStreamingLocator: OutputAsset: {outputAssetName}, ID: {id}");
            string locatorName = $"Locator-{id}";
            StreamingLocator locator = await _client.StreamingLocators.CreateAsync(
                _mediaServicesOptions.ResourceGroupName,
                _mediaServicesOptions.MediaServicesAccountName,
                locatorName,
                new StreamingLocator
                {
                    AssetName = outputAssetName,
                    StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly,
                });

            return locator;
        }

        /// <summary>
        /// Gets streaming URLs
        /// </summary>
        /// <param name="locatorName"></param>
        /// <returns></returns>
        public async Task<Dictionary<StreamingProtocol, string>> GetStreamingUrlsAsync(string locatorName)
        {
            var streamingUrls = new Dictionary<StreamingProtocol, string>();

            StreamingEndpoint streamingEndpoint = await _client.StreamingEndpoints.GetAsync(_mediaServicesOptions.ResourceGroupName, _mediaServicesOptions.MediaServicesAccountName, _mediaServicesOptions.DefaultStreamingEndpointName);

            if (streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running)
            {
                await _client.StreamingEndpoints.StartAsync(_mediaServicesOptions.ResourceGroupName, _mediaServicesOptions.MediaServicesAccountName, _mediaServicesOptions.DefaultStreamingEndpointName);
            }

            ListPathsResponse paths = await _client.StreamingLocators.ListPathsAsync(_mediaServicesOptions.ResourceGroupName, _mediaServicesOptions.MediaServicesAccountName, locatorName);

            foreach (StreamingPath path in paths.StreamingPaths)
            {
                UriBuilder uriBuilder = new UriBuilder
                {
                    Scheme = _uriScheme,
                    Host = streamingEndpoint.HostName,
                    Path = path.Paths[0]
                };

                streamingUrls.Add((StreamingProtocol)Enum.Parse(typeof(StreamingProtocol), path.StreamingProtocol, true), uriBuilder.ToString());
            }
            return streamingUrls;
        }
    }
}
