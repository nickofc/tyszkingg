using TwReplay.Services.Download;
using TwReplay.Storage.Abstraction;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpDownloadServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpDownloadService(this IServiceCollection services)
        {
            return services.AddScoped<IDownloadService, HttpDownloadService>();
        }
    }
}