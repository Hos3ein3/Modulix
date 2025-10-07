using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ModuliX.BuildingBlocks.Repositories;
using ModuliX.SharedKernel;
using ModuliX.SharedKernel.Repositories;

namespace ModuliX.BuildingBlocks.UnitOfWorks;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly IRepositoryFactory _factory;
    private IDbContextTransaction? _transaction;

    public EfUnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
        _factory = new EfRepositoryFactory(dbContext);
    }

    public IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : BaseEntity<TId>
        => _factory.CreateRepository<TEntity, TId>();

    public void BeginTransaction() => _transaction = _dbContext.Database.BeginTransaction();
    public async Task BeginTransactionAsync(CancellationToken ct = default) =>
        _transaction = await _dbContext.Database.BeginTransactionAsync(ct);

    public void Commit()
    {
        _dbContext.SaveChanges();
        _transaction?.Commit();
    }
    public async Task CommitAsync(CancellationToken ct = default)
    {
        await _dbContext.SaveChangesAsync(ct);
        if (_transaction != null)
            await _transaction.CommitAsync(ct);
    }

    public void Rollback() => _transaction?.Rollback();
    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
            await _transaction.RollbackAsync(ct);
    }

    public int SaveChanges() => _dbContext.SaveChanges();
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _dbContext.SaveChangesAsync(ct);

    public void Dispose() => _dbContext.Dispose();
    public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();
}
