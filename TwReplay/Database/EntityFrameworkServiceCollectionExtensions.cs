using System;
using Microsoft.EntityFrameworkCore;
using TwReplay.Database;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase<T>(this IServiceCollection services,
            Action<DatabaseOptions> configuration) where T : DbContext
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var databaseOptions = new DatabaseOptions();
            configuration(databaseOptions);

            return services.AddDbContext<T>(options =>
            {
                switch (databaseOptions.EngineType)
                {
                    case DatabaseEngineType.Sqlite:
                        options.UseSqlite(databaseOptions.ConnectionString);
                        break;
                    case DatabaseEngineType.PostgreSql:
                        options.UseNpgsql(databaseOptions.ConnectionString);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(databaseOptions.EngineType));
                }
            });
        }
    }
}