using System.Net.Http;
using Microsoft.Extensions.Logging;
using TwReplay.Storage.Abstraction;
using TwReplay.Storage.Videobin;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class VideobinUploadServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddVideobinUploadService(this IServiceCollection services)
        {
            return services.AddScoped<IUploadService>(provider =>
            {
                var httpClient = provider.GetRequiredService<HttpClient>();
                var logger = provider.GetRequiredService<ILogger<VideobinUploadService>>();

                return new VideobinUploadService(httpClient, logger);
            });
        }
    }
}