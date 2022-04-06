using Microsoft.EntityFrameworkCore.Storage;

namespace PublishSystem.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default(CancellationToken));
        Task CommitCurrentTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));
        void RollbackTransaction(IDbContextTransaction transaction);
        void RollbackCurrentTransaction();
        Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default(CancellationToken));
        Task RollbackCurrentTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
