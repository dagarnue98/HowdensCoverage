using PublishSystem.Domain.Enums.StateManagement;
using PublishSystem.Domain.Models;
using PublishSystem.Domain.SeedWork;

namespace PublishSystem.Domain.Repositories.PublishJobRepository
{
    public interface IPublishJobRepository : IRepositoryBase
    {
        Task<PublishJobModel> AddPublishJobAsync(PublishJobRequestModel publishJobRequestModel);

        Task<PublishJobModel?> UpdateStateToErrorAsync(Guid jobId);

        Task<PublishJobModel> UpdateStateByTriggerAsync(Guid jobId, Trigger trigger);

        Task<PublishJobModel> UpdateStateByTriggerAsync(string batchJobId, Trigger trigger);

        Task<LayerResponse<PublishJobModel>> GetPublishJobByJobIdAsync(Guid jobId);
        Task<LayerResponse<PublishJobModel>> GetPublishJobByVersionIdAsync(int versionId);
        Task<PublishJobModel?> GetPublishJobByBatchJobId(string batchJobId);

        LayerResponse<string> GetPublishStateDiagram();
    }
}
