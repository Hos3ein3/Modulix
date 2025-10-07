
using ModuliX.SharedKernel;

namespace ModuliX.BuildingBlocks.Repositories;

public interface IRepositoryFactory
{
    IRepository<TEntity, TId> CreateRepository<TEntity, TId>() where TEntity : BaseEntity<TId>;
}
