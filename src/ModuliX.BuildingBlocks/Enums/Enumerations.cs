
namespace ModuliX.BuildingBlocks.Enums;

public enum FeatureManagement
{
    alphaFeature,
    betaFeature,
}
public enum DeploymentMode
{
    Monolith = 1,
    Microservice = 2
}

public enum DatabaseStrategy
{
    SingleDb = 1,
    MultiDb = 2
}
public enum DatabaseProvider
{
    // RDBMS
    Ef = 0,        // default EF Core
    SqlServer = 1,
    PostgreSQL = 2,
    MySql = 3,
    Sqlite = 4,
    Oracle = 5,

    // NoSQL
    Mongo = 10,
    Neo4j = 11,
    Cassandra = 12,
    Redis = 13,

    // Other
    Cosmos = 20,
    InMemory = 21
}

