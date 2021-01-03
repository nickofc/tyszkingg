using TwReplay.Storage.Videobin;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class VideobinUploadServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddVideobinUploadService(this IServiceCollection services)
        {
            return services.AddScoped<VideobinUploadService>();
        }
    }
}