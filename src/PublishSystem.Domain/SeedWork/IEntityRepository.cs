namespace PublishSystem.Domain.SeedWork
{
    public interface IEntityRepository
    {
        TEntity Create<TEntity>() where TEntity : Entity;

        IQueryable<TEntity> GetAll<TEntity>() where TEntity : Entity;

        TEntity Add<TEntity>(TEntity entity) where TEntity : Entity;

        void Remove<TEntity>(TEntity entity) where TEntity : Entity;

        int Count<TEntity>() where TEntity : Entity;

        Task<int> CountAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : Entity;

        IQueryable<TEntity> Where<TEntity>(ISpecification<TEntity> specification) where TEntity : Entity;

        bool Any<TEntity>(ISpecification<TEntity> specification) where TEntity : Entity;

        Task<bool> AnyAsync<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default) where TEntity : Entity;

        TEntity? FindById<TEntity>(int id) where TEntity : Entity;

        Task<TEntity?> FindByIdAsync<TEntity>(int id, CancellationToken cancellationToken = default) where TEntity : Entity;

        Task RemoveAsync<TEntity>(int key, CancellationToken cancellationToken = default) where TEntity : Entity;
    }

    public interface IEntityRepository<TEntity>
        where TEntity : Entity
    {
        TEntity Create();

        IQueryable GetAll();

        TEntity Add(TEntity entity);

        void Remove(TEntity entity);

        int Count();

        Task<int> CountAsync(CancellationToken cancellationToken = default);

        IQueryable<TEntity> Where(ISpecification<TEntity> specification);

        bool Any(ISpecification<TEntity> specification);

        Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        TEntity? FindById(int id);

        Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default);

        Task RemoveAsync(int key, CancellationToken cancellationToken = default);
    }
}