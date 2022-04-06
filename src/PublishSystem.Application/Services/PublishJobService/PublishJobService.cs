namespace PublishSystem.Application.Services.PublishJobService
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

    public class PublishJobService : ServiceBase<PublishJobService>, IPublishJobService
    {
        private readonly IPublishJobRepository _publishJobRepository;

        public PublishJobService(IPublishJobRepository publishJobRepository,
            ILogger<PublishJobService> logger, IMapper mapper, IUnitOfWork unitOfWork, IEventBus eventBus)
            : base(logger, mapper, unitOfWork, eventBus)
        {
            _publishJobRepository = publishJobRepository;
        }

        public async Task<LayerResponse<PublishJobModel>> AddPublishJobAsync(PublishJobRequestModel publishJobRequestModel)
        {
            await _unitOfWork.BeginTransactionAsync();
            var publishJobModel = await _publishJobRepository.AddPublishJobAsync(publishJobRequestModel);
            await _unitOfWork.CommitAsync();

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                publishJobModel = await _publishJobRepository.UpdateStateByTriggerAsync(publishJobModel.JobId, Trigger.Send);
                await _unitOfWork.CommitAsync();
                await SendRenderingRequestEventAsync(publishJobModel.VersionId, publishJobModel.JobId);
            }
            catch (Exception)
            {
                publishJobModel = await _publishJobRepository.UpdateStateByTriggerAsync(publishJobModel.JobId, Trigger.Error);
                await _unitOfWork.CommitAsync();
                throw;
            }

            return new LayerResponse<PublishJobModel>(publishJobModel);
        }

        public async Task<LayerResponse<PublishJobModel>> UpdateEncodeStatePublishRequestAsync(TaskEvent taskEventBody)
        {
            await _unitOfWork.BeginTransactionAsync();

            var publishRequestModel = await _publishJobRepository.GetPublishJobByBatchJobId(taskEventBody.JobId);
            var exitCode = GetExitCode(taskEventBody);

            if (exitCode != ExitCode.Start && publishRequestModel != null)
            {
                publishRequestModel.State = (exitCode is ExitCode.Complete)
                    ? _publishJobRepository.UpdateStateByTriggerAsync(taskEventBody.JobId, Trigger.Subscribe).ToString() 
                    : _publishJobRepository.UpdateStateByTriggerAsync(taskEventBody.JobId, Trigger.Error).ToString();
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public LayerResponse<string> GetPublishStateDiagram()
        {
            _logger.LogDebug("Getting publish state diagram");
            return _publishJobRepository.GetPublishStateDiagram();
        }

        private async Task SendRenderingRequestEventAsync(int versionId, Guid jobId)
        {
            var renderingRequestEvent = new RenderingRequestEvent
            {
                JobId = jobId,
                VersionId = versionId,
            };

            await _eventBus.SendAsync(renderingRequestEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public async Task<LayerResponse<PublishJobModel>> GetPublishJobByVersionIdAsync(int versionId)
        {
            _logger.LogDebug($"GetPublishJobByVersionIdAsync() called with version ID {versionId}");
            return await _publishJobRepository.GetPublishJobByVersionIdAsync(versionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<LayerResponse<PublishJobModel>> GetPublishJobByJobIdAsync(Guid jobId)
        {
            _logger.LogDebug($"GetPublishJobByJobIdAsync() called with job ID {jobId}");
            return await _publishJobRepository.GetPublishJobByJobIdAsync(jobId);
        }
    }
}