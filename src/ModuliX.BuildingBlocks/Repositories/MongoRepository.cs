using System.Linq.Expressions;
using ModuliX.SharedKernel;
using ModuliX.SharedKernel.Contracts;
using MongoDB.Driver;
namespace ModuliX.BuildingBlocks.Repositories;

public class MongoRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, IBaseEntity<TId>
{
    private readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<TEntity>(collectionName);
    }

    public TEntity? GetById(TId id) =>
        _collection.Find(x => x.Id!.Equals(id) && !x.IsDeleted).FirstOrDefault();

    // public TEntity? GetByTrackId(Guid trackId) =>
    //     _collection.Find(x => x.TrackId == trackId && !x.IsDeleted).FirstOrDefault();

    public IReadOnlyList<TEntity> GetAll() =>
        _collection.Find(x => !x.IsDeleted).ToList();

    public IReadOnlyList<TEntity> Find(Expression<Func<TEntity, bool>> predicate) =>
        _collection.Find(Builders<TEntity>.Filter.Where(predicate) & Builders<TEntity>.Filter.Eq(x => x.IsDeleted, false)).ToList();

    public void Add(TEntity entity) => _collection.InsertOne(entity);
    public void Update(TEntity entity) => _collection.ReplaceOne(x => x.Id!.Equals(entity.Id), entity);

    public void Delete(TEntity entity, Guid deletedByUserId)
    {
        entity.MarkDeleted(deletedByUserId.ToString());
        Update(entity);
    }

    // Async versions
    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default) =>
        await _collection.Find(x => x.Id!.Equals(id) && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);

    // public async Task<TEntity?> GetByTrackIdAsync(Guid trackId, CancellationToken cancellationToken = default) =>
    //     await _collection.Find(x => x.TrackId == trackId && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _collection.Find(x => !x.IsDeleted).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _collection.Find(Builders<TEntity>.Filter.Where(predicate) & Builders<TEntity>.Filter.Eq(x => x.IsDeleted, false)).ToListAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _collection.ReplaceOneAsync(x => x.Id!.Equals(entity.Id), entity, cancellationToken: cancellationToken);

    public async Task DeleteAsync(TEntity entity, Guid deletedByUserId, CancellationToken cancellationToken = default)
    {
        entity.MarkDeleted(deletedByUserId.ToString());
        await UpdateAsync(entity, cancellationToken);
    }

    // Bulk methods
    public void AddRange(IEnumerable<TEntity> entities) => _collection.InsertMany(entities);
    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) => await _collection.InsertManyAsync(entities, cancellationToken: cancellationToken);

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            Update(entity);
    }
    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
            await UpdateAsync(entity, cancellationToken);
    }

    public void DeleteRange(IEnumerable<TEntity> entities, Guid deletedByUserId)
    {
        foreach (var entity in entities)
            Delete(entity, deletedByUserId);
    }
    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, Guid deletedByUserId, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
            await DeleteAsync(entity, deletedByUserId, cancellationToken);
    }

    // Mongo doesn't use SaveChanges (operations are immediate), so return 0
    public int SaveChanges() => 0;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => Task.FromResult(0);
}
