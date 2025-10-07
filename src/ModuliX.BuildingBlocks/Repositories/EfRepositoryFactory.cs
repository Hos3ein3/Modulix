using Microsoft.EntityFrameworkCore;
using ModuliX.SharedKernel;
using ModuliX.SharedKernel.Repositories;

namespace ModuliX.BuildingBlocks.Repositories;

public class EfRepositoryFactory : IRepositoryFactory
{
    private readonly DbContext _dbContext;

    public EfRepositoryFactory(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IRepository<TEntity, TId> CreateRepository<TEntity, TId>() where TEntity : BaseEntity<TId>
        => new EfRepository<TEntity, TId>(_dbContext);
}
