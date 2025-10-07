
using System.Collections.Concurrent;
using ModuliX.BuildingBlocks.Repositories;
using ModuliX.SharedKernel;
using MongoDB.Driver;

namespace ModuliX.BuildingBlocks.UnitOfWorks;

public class MongoUnitOfWork : IUnitOfWork
{
    private readonly IMongoDatabase _database;
    private readonly IRepositoryFactory _factory;
    private IClientSessionHandle? _session;

    public MongoUnitOfWork(IMongoDatabase database)
    {
        _database = database;
        _factory = new MongoRepositoryFactory(database);
    }

    public IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : BaseEntity<TId>
        => _factory.CreateRepository<TEntity, TId>();

    public void BeginTransaction()
    {
        var client = _database.Client;
        _session = client.StartSession();
        _session.StartTransaction();
    }
    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        var client = _database.Client;
        _session = await client.StartSessionAsync(cancellationToken: ct);
        _session.StartTransaction();
    }

    public void Commit() => _session?.CommitTransaction();
    public async Task CommitAsync(CancellationToken ct = default) => await _session?.CommitTransactionAsync(ct)!;

    public void Rollback() => _session?.AbortTransaction();
    public async Task RollbackAsync(CancellationToken ct = default) => await _session?.AbortTransactionAsync(ct)!;

    public int SaveChanges() => 0; // Mongo writes immediately
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => Task.FromResult(0);

    public void Dispose() => _session?.Dispose();
    public ValueTask DisposeAsync()
    {
        _session?.Dispose();
        return ValueTask.CompletedTask;
    }
}
