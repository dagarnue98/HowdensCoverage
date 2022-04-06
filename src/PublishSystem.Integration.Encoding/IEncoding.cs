using PublishSystem.Integration.Encoding.Enums;
using PublishSystem.Integration.Encoding.Models;

namespace PublishSystem.Integration.Encoding
{
    public interface IEncoding
    {
        Task<EncodingJob> CreateEncodingJobAsync(string id, string inputAssetLocation);
        Task<JobState> GetJobStateAsync(string jobName);
        Task<Dictionary<StreamingProtocol, string>> GetStreamingUrlsAsync(string outputAssetName, string id);
    }
}
