
using ModuliX.SharedKernel;
using MongoDB.Driver;

namespace ModuliX.BuildingBlocks.Repositories;

public class MongoRepositoryFactory : IRepositoryFactory
{
    private readonly IMongoDatabase _database;

    public MongoRepositoryFactory(IMongoDatabase database)
    {
        _database = database;
    }

    public IRepository<TEntity, TId> CreateRepository<TEntity, TId>() where TEntity : BaseEntity<TId>
        => new MongoRepository<TEntity, TId>(_database, typeof(TEntity).Name.ToLowerInvariant() + "s");
}
