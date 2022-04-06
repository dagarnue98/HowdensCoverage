using AutoMapper;
using Microsoft.Extensions.Logging;

namespace PublishSystem.Domain.SeedWork
{
    public abstract class RepositoryBase<TRepository, TEntity>
        where TRepository : IRepositoryBase
        where TEntity : Entity
    {
        protected readonly IEntityRepository<TEntity> _entityRepository;
        protected readonly ILogger<TRepository> _logger;
        protected readonly IMapper _mapper;

        public RepositoryBase(ILogger<TRepository> logger, IMapper mapper, IEntityRepository<TEntity> entityRepository)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        }
    }
}
