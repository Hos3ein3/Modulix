using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ModuliX.BuildingBlocks.Repositories;
using ModuliX.SharedKernel.Contracts;

namespace ModuliX.SharedKernel.Repositories;

public class EfRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, IBaseEntity<TId>
{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public EfRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default) =>
        await _dbSet.FirstOrDefaultAsync(e => e.Id!.Equals(id) && !e.IsDeleted, cancellationToken);

    // public async Task<TEntity?> GetByTrackIdAsync(Guid trackId, CancellationToken cancellationToken = default) =>
    //     await _dbSet.FirstOrDefaultAsync(e => e.TrackId == trackId && !e.IsDeleted, cancellationToken);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbSet.Where(e => !e.IsDeleted).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _dbSet.Where(e => !e.IsDeleted).Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity, Guid deletedByUserId, CancellationToken cancellationToken = default)
    {
        entity.MarkDeleted(deletedByUserId.ToString()); // your BaseEntity delete logic
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.SaveChangesAsync(cancellationToken);
    public TEntity? GetById(TId id) =>
    _dbSet.FirstOrDefault(e => e.Id!.Equals(id) && !e.IsDeleted);

    // public TEntity? GetByTrackId(Guid trackId) =>
    //     _dbSet.FirstOrDefault(e => e.TrackId == trackId && !e.IsDeleted);

    public IReadOnlyList<TEntity> GetAll() =>
        _dbSet.Where(e => !e.IsDeleted).ToList();

    public IReadOnlyList<TEntity> Find(Expression<Func<TEntity, bool>> predicate) =>
        _dbSet.Where(e => !e.IsDeleted).Where(predicate).ToList();

    public void Add(TEntity entity) =>
        _dbSet.Add(entity);

    public void Update(TEntity entity) =>
        _dbSet.Update(entity);

    public void Delete(TEntity entity, Guid deletedByUserId)
    {
        entity.MarkDeleted(deletedByUserId.ToString()); // mark soft delete
        _dbSet.Update(entity);
    }

    public int SaveChanges() =>
        _dbContext.SaveChanges();

    // -------------------------------
    // Bulk operations (sync)
    // -------------------------------
    public void AddRange(IEnumerable<TEntity> entities) =>
        _dbSet.AddRange(entities);

    public void UpdateRange(IEnumerable<TEntity> entities) =>
        _dbSet.UpdateRange(entities);

    public void DeleteRange(IEnumerable<TEntity> entities, Guid deletedByUserId)
    {
        foreach (var entity in entities)
        {
            entity.MarkDeleted(deletedByUserId.ToString());
        }
        _dbSet.UpdateRange(entities);
    }

    // -------------------------------
    // Bulk operations (async)
    // -------------------------------
    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
        _dbSet.AddRangeAsync(entities, cancellationToken);

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, Guid deletedByUserId, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.MarkDeleted(deletedByUserId.ToString());
        }
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

}
