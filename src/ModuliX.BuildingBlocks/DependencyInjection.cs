
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuliX.BuildingBlocks.Enums;
using ModuliX.BuildingBlocks.Repositories;
using ModuliX.BuildingBlocks.UnitOfWorks;
using ModuliX.SharedKernel.Repositories;

// Notes:
// - Supported providers via config: "Ef" (default), "SqlServer", "PostgreSQL", "MySql", "Sqlite", "Oracle", "Mongo", "Neo4j".
// - You MUST register the underlying client before calling this extension:
//     EF:     services.AddDbContext<AppDbContext>(...);
//     Mongo:  services.AddSingleton<IMongoClient>(...); services.AddScoped<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("YourDb"));
//     Neo4j:  services.AddSingleton<IDriver>(GraphDatabase.Driver(uri, AuthTokens.Basic(user, pass)));
//
// Usage in Program.cs:
//     builder.Services.AddModuliXPersistence(builder.Configuration);
//
// appsettings.json:
//   "Database": {
//     "Provider": "SqlServer" // Ef/SqlServer/PostgreSQL/MySql/Sqlite/Oracle/Mongo/Neo4j
//   }

namespace ModuliX.BuildingBlocks
{
    public static class DependencyInjection
    {

        /// <summary>
        /// Registers IUnitOfWork + IRepositoryFactory according to Database:Provider.
        /// EF default with generic IRepository&lt;,&gt; binding; Mongo/Neo4j via factories only.
        /// </summary>
        public static IServiceCollection AddModuliXPersistence(
            this IServiceCollection services,
            IConfiguration configuration,
            bool registerEfGenericRepository = true)
        {
            var providerString = configuration["Database:Provider"] ?? "Ef";
            if (!Enum.TryParse<DatabaseProvider>(providerString, true, out var provider))
                provider = DatabaseProvider.Ef;

            switch (provider)
            {
                case DatabaseProvider.Mongo:
                    services.AddScoped<IRepositoryFactory, MongoRepositoryFactory>();
                    services.AddScoped<IUnitOfWork, MongoUnitOfWork>();
                    break;

                case DatabaseProvider.Neo4j:
                    // services.AddScoped<IRepositoryFactory, Neo4jRepositoryFactory>();
                    // services.AddScoped<IUnitOfWork, Neo4jUnitOfWork>();
                    break;

                default: // All EF-compatible providers
                    services.AddScoped<IRepositoryFactory, EfRepositoryFactory>();
                    services.AddScoped<IUnitOfWork, EfUnitOfWork>();
                    services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
                    break;
            }

            return services;
        }
    }
}
