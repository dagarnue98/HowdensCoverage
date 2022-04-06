using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublishSystem.Domain.BusinessRules.StateManager;
using PublishSystem.Domain.Entities;
using PublishSystem.Domain.Enums.Error;
using PublishSystem.Domain.Enums.StateManagement;
using PublishSystem.Domain.Models;
using PublishSystem.Domain.SeedWork;
using PublishSystem.Domain.Specification.PublishJobSpecification;
using PublishSystem.Domain.Specification.PublishRequestSpecification;
using PublishSystem.Domain.Specifications.PublishJobSpecification;

namespace PublishSystem.Domain.Repositories.PublishJobRepository
{
    public class PublishJobRepository : RepositoryBase<PublishJobRepository, PublishJob>, IPublishJobRepository
    {
        private readonly IStateManager _stateManagementService;

        public PublishJobRepository(ILogger<PublishJobRepository> logger, IMapper mapper, IUnitOfWork unitOfWork, IEntityRepository<PublishJob> entityRepository,
            IStateManager stateManagementService)
            : base(logger, mapper, entityRepository)
        {
            _stateManagementService = stateManagementService;
        }

        public async Task<PublishJobModel> AddPublishJobAsync(PublishJobRequestModel publishJobRequestModel)
        {
            var publishJobEntity = _mapper.Map<PublishJob>(publishJobRequestModel);
            publishJobEntity.JobId = Guid.NewGuid();
            publishJobEntity.State = _stateManagementService.InitialiseState(State.Requested).ToString();
            publishJobEntity = _entityRepository.Add(publishJobEntity);
            var publishJobModel = _mapper.Map<PublishJob, PublishJobModel>(publishJobEntity);
            return publishJobModel;
        }

        public async Task<PublishJobModel?> UpdateStateToErrorAsync(Guid jobId)
        {
            PublishJobModel? publishJobModel = null;
            var publishJobEntity = await GetByJobIdAsync(jobId);
            if (publishJobEntity != null)
            {
                publishJobEntity.State = _stateManagementService.ChangeState(Trigger.Error).ToString();

                publishJobModel = _mapper.Map<PublishJob, PublishJobModel>(publishJobEntity);
            }

            return publishJobModel;
        }

        public async Task<PublishJobModel> UpdateStateByTriggerAsync(Guid jobId, Trigger trigger)
        {
            PublishJob? publishJobEntity = await GetByJobIdAsync(jobId);

            if (publishJobEntity == null)
            {
                throw new KeyNotFoundException($"DB record not found. JobId: {jobId} - Table 'PublishJob'.");
            }

            var currentState = Enum.Parse<State>(publishJobEntity.State);
            if (currentState != _stateManagementService.CurrentState)
            {
                _stateManagementService.InitialiseState(currentState);
            }

            var newState = _stateManagementService.ChangeState(trigger);
            publishJobEntity.State = newState.ToString();
            var publishJobModel = _mapper.Map<PublishJob, PublishJobModel>(publishJobEntity);
            return publishJobModel;
        }

        public async Task<PublishJobModel> UpdateStateByTriggerAsync(string batchJobId, Trigger trigger)
        {
            PublishJob? publishJobEntity = await GetByBatchJobId(batchJobId);

            if (publishJobEntity == null)
            {
                throw new KeyNotFoundException($"DB record not found. BatchJobId: {batchJobId} - Table 'PublishJob'.");
            }

            var currentState = System.Enum.Parse<State>(publishJobEntity.State);
            if (currentState != _stateManagementService.CurrentState)
            {
                _stateManagementService.InitialiseState(currentState);
            }

            var newState = _stateManagementService.ChangeState(trigger);
            publishJobEntity.State = newState.ToString();


            var publishJobModel = _mapper.Map<PublishJob, PublishJobModel>(publishJobEntity);
            return publishJobModel;
        }

        public async Task<LayerResponse<PublishJobModel>> GetPublishJobByJobIdAsync(Guid jobId)
        {
            var repositoryResponse = new LayerResponse<PublishJobModel>();
            var publishJobEntity = await GetByJobIdAsync(jobId);

            if (publishJobEntity is null)
            {
                repositoryResponse.AddError(ErrorCode.EntityNotFound, $"No publish job with job ID {jobId} found");
                _logger.LogDebug($"No entity found with job ID {jobId}");
                return repositoryResponse;
            }
            repositoryResponse.Content = _mapper.Map<PublishJob, PublishJobModel>(publishJobEntity);
            _logger.LogDebug($"Completed call to GetPublishJobByJobIdAsync() with jobId ID {jobId}");
            return repositoryResponse;
        }

        public async Task<PublishJobModel?> GetPublishJobByBatchJobId(string batchJobId)
        {
            PublishJobModel? publishJobModel = null;
            var publishJobEntity = await GetByBatchJobId(batchJobId);
            if (publishJobEntity != null)
            {
                publishJobModel = _mapper.Map<PublishJob, PublishJobModel>(publishJobEntity);
            }

            return publishJobModel;
        }

        private async Task<PublishJob?> GetByJobIdAsync(Guid jobId)
        {
            var publishJobEntity = await _entityRepository.Where(new PublishJobByJobIdSpecification(jobId)).SingleOrDefaultAsync();
            return publishJobEntity;
        }

        private async Task<PublishJob?> GetByBatchJobId(string batchJobId)
        {
            var publishJob = await _entityRepository.Where(new PublishJobByBatchJobIdSpecification(batchJobId)).SingleOrDefaultAsync();
            return publishJob;
        }

        /// <summary>
        /// Gets a publish job by the plan version ID
        /// </summary>
        /// <param name="versionId">The plan version ID to be queried</param>
        /// <returns>Returns a PublishJobModel</returns>
        public async Task<LayerResponse<PublishJobModel>> GetPublishJobByVersionIdAsync(int versionId)
        {
            var repositoryResponse = new LayerResponse<PublishJobModel>();

            var publishJobEntity = await _entityRepository.Where(new PublishJobByVersionIdSpecification(versionId)).SingleOrDefaultAsync();
            if (publishJobEntity is null)
            {
                repositoryResponse.AddError(ErrorCode.EntityNotFound, $"No publish job with version ID {versionId} found");
                _logger.LogDebug($"No entity found with version ID {versionId}");
                return repositoryResponse;
            }
            repositoryResponse.Content = _mapper.Map<PublishJob, PublishJobModel>(publishJobEntity);
            _logger.LogDebug($"Completed call to GetPublishJobByVersionIdAsync() with version ID {versionId}");
            return repositoryResponse;
        }

    /// <summary>
    /// Get Dot Graph string of state diagram
    /// </summary>
    /// <returns></returns>
        public LayerResponse<string> GetPublishStateDiagram()
        {
            _stateManagementService.InitialiseState(State.Requested);
            return new LayerResponse<string> { Content = _stateManagementService.ToDotGraph() };
        }
    }
}
