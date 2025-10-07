
using Microsoft.EntityFrameworkCore;

namespace ModuliX.BuildingBlocks.Context;

public interface IModuleDbContextConfigurator
{
    void Configure(ModelBuilder modelBuilder);
    void RegisterEntities(ModelBuilder modelBuilder);
}
