

namespace PublishSystem.Application.Services.PublishJobService
{
    using PublishSystem.Domain.Models;
    using PublishSystem.Domain.SeedWork;
    using PublishSystem.Integration.Rendering.AzureBatch.Events;

    public interface IPublishJobService : IServiceBase
    {
        Task<LayerResponse<PublishJobModel>> UpdateEncodeStatePublishRequestAsync(TaskEvent taskEventBody);

        Task<LayerResponse<PublishJobModel>> AddPublishJobAsync(PublishJobRequestModel publishJobModel);

        Task<LayerResponse<PublishJobModel>> GetPublishJobByVersionIdAsync(int versionId);

        Task<LayerResponse<PublishJobModel>> GetPublishJobByJobIdAsync(Guid guid);

        LayerResponse<string> GetPublishStateDiagram();
    }
}
