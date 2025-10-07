using Microsoft.EntityFrameworkCore;
using ModuliX.BuildingBlocks.Context;

namespace ModuliX.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    private readonly IEnumerable<IModuleDbContextConfigurator> _configurators;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                        IEnumerable<IModuleDbContextConfigurator> configurators)
        : base(options)
    {
        _configurators = configurators;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var configurator in _configurators)
        {
            configurator.Configure(modelBuilder);
        }
        base.OnModelCreating(modelBuilder);
    }
}
