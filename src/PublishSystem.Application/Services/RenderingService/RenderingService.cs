namespace PublishSystem.Application.Services.RenderingService
{
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using PublishSystem.Application.Events;
    using PublishSystem.Domain.Enums.ExitCode;
    using PublishSystem.Domain.Enums.StateManagement;
    using PublishSystem.Domain.Models;
    using PublishSystem.Domain.Repositories.PublishJobRepository;
    using PublishSystem.Domain.SeedWork;
    using PublishSystem.Integration.Messaging;
    using PublishSystem.Integration.Rendering.AzureBatch.Events;

    public class RenderingService : ServiceBase<RenderingService>, IRenderingService
    {
        private readonly IPublishJobRepository _publishJobRepository;

        public RenderingService(
            IPublishJobRepository publishJobRepository,
            ILogger<RenderingService> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IEventBus eventBus)
            : base(logger, mapper, unitOfWork, eventBus)
        {
            _publishJobRepository = publishJobRepository;
        }

        public async Task<LayerResponse<PublishJobModel>> StartRenderingAsync(RenderingRequestModel renderingRequestModel)
        {
            await _unitOfWork.BeginTransactionAsync();
            var publishJobModel = await _publishJobRepository.UpdateStateByTriggerAsync(renderingRequestModel.JobId, Trigger.Subscribe);
            await _unitOfWork.CommitAsync();

            return new LayerResponse<PublishJobModel>(publishJobModel);
        }

        public async Task<LayerResponse<PublishJobModel>> HandlerRenderingEventAsync(TaskEvent taskEventBody)
        {
            await _unitOfWork.BeginTransactionAsync();

            var publishRequestModel = await _publishJobRepository.GetPublishJobByBatchJobId(taskEventBody.JobId);
            var exitCode = GetExitCode(taskEventBody);

            if (exitCode != ExitCode.Start && publishRequestModel != null)
            {
                publishRequestModel.State = (exitCode is ExitCode.Complete)
                    ? (await _publishJobRepository.UpdateStateByTriggerAsync(taskEventBody.JobId, Trigger.Completed)).State
                    : (await _publishJobRepository.UpdateStateByTriggerAsync(taskEventBody.JobId, Trigger.Error)).State;
            }

            await _unitOfWork.CommitAsync();

            return new LayerResponse<PublishJobModel>(publishRequestModel);
        }

        private static ExitCode GetExitCode(TaskEvent taskEventBody)
        {
            ExitCode exitCode;
            if (taskEventBody.ExecutionInfo is null)
            {
                exitCode = ExitCode.ScheduleFail;
            }
            else if (taskEventBody.ExecutionInfo.ExitCode is null)
            {
                exitCode = ExitCode.Start;
            }
            else
            {
                exitCode = (ExitCode)taskEventBody.ExecutionInfo.ExitCode;
            }

            return exitCode;
        }

        private async Task SendEncodingRequestEventAsync(int versionId, Guid jobId)
        {
            var encodingRequestEvent = new EncodingRequestEvent
            {
                JobId = jobId,
                VersionId = versionId,
            };

            await _eventBus.SendAsync(encodingRequestEvent);
        }
    }
}
