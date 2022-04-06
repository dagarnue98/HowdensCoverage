using Microsoft.Azure.Management.Media.Models;
using PublishSystem.Integration.Encoding.Enums;

namespace PublishSystem.Integration.Encoding.Services
{
    public interface IMediaService
    {
        Task<string> CreateOutputAssetAsync(string id);
        Task<Job> SubmitJobAsync(string outputAssetName, string id, string blobLocationUrl);
        Task<Job> GetJobAsync(string jobName);
        Task<StreamingLocator> CreateStreamingLocatorAsync(string outputAssetName, string id);
        Task<Dictionary<StreamingProtocol, string>> GetStreamingUrlsAsync(string locatorName);
    }
}
