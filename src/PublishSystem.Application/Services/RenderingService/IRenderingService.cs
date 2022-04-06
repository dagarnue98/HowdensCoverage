using PublishSystem.Domain.Models;
using PublishSystem.Domain.SeedWork;
using PublishSystem.Integration.Rendering.AzureBatch.Events;

namespace PublishSystem.Application.Services.RenderingService
{
    public interface IRenderingService : IServiceBase
    {
        Task<LayerResponse<PublishJobModel>> StartRenderingAsync(RenderingRequestModel renderingRequestModel);

        Task<LayerResponse<PublishJobModel>> HandlerRenderingEventAsync(TaskEvent taskEventBody);
    }
}