using Microsoft.EntityFrameworkCore;

namespace PublishSystem.Domain.SeedWork
{
    public class EntityRepository : IEntityRepository
    {
        private readonly DbContext _dataContext;

        public EntityRepository(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        private DbContext Context => this._dataContext;

        public virtual TEntity Create<TEntity>()
            where TEntity : Entity
        {
            return Activator.CreateInstance<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll<TEntity>()
            where TEntity : Entity
        {
            return this.GetQuery<TEntity>();
        }

        public virtual TEntity Add<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            var entry = this.Context.Set<TEntity>().Add(entity);
            return entry.Entity;
        }

        public virtual void Remove<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            this.Context.Set<TEntity>().Remove(entity);
        }

        public virtual int Count<TEntity>()
            where TEntity : Entity
        {
            return this.GetQuery<TEntity>().Count();
        }

        public virtual async Task<int> CountAsync<TEntity>(CancellationToken cancellationTEntityoken = default)
            where TEntity : Entity
        {
            return await this.GetQuery<TEntity>().CountAsync(cancellationTEntityoken);
        }

        public virtual IQueryable<TEntity> Where<TEntity>(ISpecification<TEntity> specification)
            where TEntity : Entity
        {
            return this.GetQuery<TEntity>().Where(specification.Predicate);
        }

        public virtual bool Any<TEntity>(ISpecification<TEntity> specification)
            where TEntity : Entity
        {
            return this.GetQuery<TEntity>().Any(specification.Predicate);
        }

        public virtual async Task<bool> AnyAsync<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationTEntityoken = default)
            where TEntity : Entity
        {
            return await this.GetQuery<TEntity>().AnyAsync(specification.Predicate, cancellationTEntityoken);
        }

        public virtual TEntity? FindById<TEntity>(int id) where TEntity : Entity
        {
            var queryable = this.GetQuery<TEntity>().Where(x => x.Id == id);
            return queryable.SingleOrDefault();
        }

        public virtual async Task<TEntity?> FindByIdAsync<TEntity>(int id, CancellationToken cancellationTEntityoken = default)
            where TEntity : Entity
        {
            var queryable = this.GetQuery<TEntity>().Where(x => x.Id == id);
            return await queryable.SingleOrDefaultAsync(cancellationTEntityoken);
        }

        public virtual async Task RemoveAsync<TEntity>(int key, CancellationToken cancellationTEntityoken = default)
            where TEntity : Entity
        {
            var entity = await this.FindByIdAsync<TEntity>(key, cancellationTEntityoken);
            if (entity == null)
            {
                throw new ArgumentException(nameof(key));
            }

            this.Remove(entity);
        }

        public void Remove<TEntity>(int key)
            where TEntity : Entity
        {
            var entity = this.FindById<TEntity>(key);
            if (entity == null)
            {
                throw new ArgumentException(nameof(key));
            }

            this.Remove(entity);
        }

        protected virtual IQueryable<TEntity> GetQuery<TEntity>()
            where TEntity : Entity
        {
            return this.Context.Set<TEntity>();
        }
    }

    public class EntityRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : Entity
    {
        private readonly IEntityRepository _entityRepository;

        public EntityRepository(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public virtual TEntity Add(TEntity entity) => _entityRepository.Add<TEntity>(entity);

        public virtual bool Any(ISpecification<TEntity> specification) => _entityRepository.Any<TEntity>(specification);

        public virtual async Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationTEntityoken = default) => await _entityRepository.AnyAsync(specification, cancellationTEntityoken);

        public virtual int Count()
        {
            var count = _entityRepository.Count<TEntity>();
            return count;
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationTEntityoken = default)
        {
            return await _entityRepository.CountAsync<TEntity>(cancellationTEntityoken);
        }

        public virtual TEntity Create() => _entityRepository.Create<TEntity>();

        public virtual TEntity? FindById(int id) => _entityRepository.FindById<TEntity>(id);

        public virtual async Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationTEntityoken = default) => await _entityRepository.FindByIdAsync<TEntity>(id, cancellationTEntityoken);

        public virtual IQueryable GetAll() => _entityRepository.GetAll<TEntity>();

        public virtual void Remove(TEntity entity) => _entityRepository.Remove<TEntity>(entity);

        public virtual async Task RemoveAsync(int key, CancellationToken cancellationTEntityoken = default) => await _entityRepository.RemoveAsync<TEntity>(key, cancellationTEntityoken);

        public virtual IQueryable<TEntity> Where(ISpecification<TEntity> specification) => _entityRepository.Where<TEntity>(specification);
    }
}
