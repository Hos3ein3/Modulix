
using ModuliX.BuildingBlocks.Enums;

namespace ModuliX.BuildingBlocks.Configurations;

public class DatabaseConfigurations
{
    public string? ConnectionString { get; set; }
    public DatabaseProvider Provider { get; set; } = DatabaseProvider.Ef;
}
