using ModuliX.BuildingBlocks.Repositories;
using ModuliX.SharedKernel;

namespace ModuliX.BuildingBlocks.UnitOfWorks;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : BaseEntity<TId>;

    // Transactions
    void BeginTransaction();
    Task BeginTransactionAsync(CancellationToken ct = default);

    void Commit();
    Task CommitAsync(CancellationToken ct = default);

    void Rollback();
    Task RollbackAsync(CancellationToken ct = default);

    // Save Changes
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
