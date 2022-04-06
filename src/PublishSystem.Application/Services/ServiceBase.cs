using AutoMapper;
using Microsoft.Extensions.Logging;
using PublishSystem.Domain.SeedWork;
using PublishSystem.Integration.Messaging;

namespace PublishSystem.Application.Services
{
    public abstract class ServiceBase<T>
        where T : IServiceBase
    {
        protected readonly IEventBus _eventBus;
        protected readonly ILogger<T> _logger;
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _unitOfWork;

        public ServiceBase(ILogger<T> logger, IMapper mapper, IUnitOfWork unitOfWork, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }
    }
}
