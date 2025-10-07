
using System.Linq.Expressions;
using ModuliX.SharedKernel;
using ModuliX.SharedKernel.Contracts;

namespace ModuliX.BuildingBlocks.Repositories;

public interface IRepository<TEntity, TId> where TEntity : IBaseEntity<TId>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    //Task<TEntity?> GetByTrackIdAsync(Guid trackId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, Guid deletedByUserId, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    TEntity? GetById(TId id);
    //TEntity? GetByTrackId(Guid trackId);
    IReadOnlyList<TEntity> GetAll();
    IReadOnlyList<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity, Guid deletedByUserId);

    int SaveChanges();
    void AddRange(IEnumerable<TEntity> entities);
    void UpdateRange(IEnumerable<TEntity> entities);
    void DeleteRange(IEnumerable<TEntity> entities, Guid deletedByUserId);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, Guid deletedByUserId, CancellationToken cancellationToken = default);
}
