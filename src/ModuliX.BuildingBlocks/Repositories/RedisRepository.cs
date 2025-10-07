using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using ModuliX.SharedKernel.Contracts;

namespace ModuliX.BuildingBlocks.Repositories;

public class RedisRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class, IBaseEntity<TId>
{
    private readonly IDistributedCache _cache;
    private readonly string _prefix;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public RedisRepository(IDistributedCache cache)
    {
        _cache = cache;
        _prefix = typeof(TEntity).Name + ":";
    }

    private string GetKey(TId id) => $"{_prefix}{id}";

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var data = await _cache.GetStringAsync(GetKey(id), cancellationToken);
        return data is null ? null : JsonSerializer.Deserialize<TEntity>(data, _jsonOptions);
    }

    [Obsolete("GetByTrackIdAsync is not supported in RedisRepository")]
    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("RedisRepository does not support full listing operations directly.");
    }

    [Obsolete("GetByTrackIdAsync is not supported in RedisRepository")]
    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Filtering is not directly supported in Redis.");
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _cache.SetStringAsync(GetKey(entity.Id), JsonSerializer.Serialize(entity, _jsonOptions), cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, Guid deletedByUserId, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(GetKey(entity.Id), cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Redis operations are immediate; nothing to save.
        return await Task.FromResult(0);
    }

    // ===== Synchronous versions =====
    public TEntity? GetById(TId id)
    {
        var data = _cache.GetString(GetKey(id));
        return data is null ? null : JsonSerializer.Deserialize<TEntity>(data, _jsonOptions);
    }

    public IReadOnlyList<TEntity> GetAll() => throw new NotSupportedException();
    public IReadOnlyList<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => throw new NotSupportedException();

    public void Add(TEntity entity) => _cache.SetString(GetKey(entity.Id), JsonSerializer.Serialize(entity, _jsonOptions));
    public void Update(TEntity entity) => Add(entity);
    public void Delete(TEntity entity, Guid deletedByUserId) => _cache.Remove(GetKey(entity.Id));
    public int SaveChanges() => 0;

    public void AddRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            Add(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            Update(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities, Guid deletedByUserId)
    {
        foreach (var entity in entities)
            Delete(entity, deletedByUserId);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
            await AddAsync(entity, cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
            await UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, Guid deletedByUserId, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
            await DeleteAsync(entity, deletedByUserId, cancellationToken);
    }
}
