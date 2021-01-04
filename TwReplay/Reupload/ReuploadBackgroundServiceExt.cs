using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TwReplay.Services
{
    public static class ReuploadBackgroundServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddReuploadBackgroundService(this IServiceCollection services,
            Action<ReuploadBackgroundConfiguration> configuration)
        {
            var reuploadBackgroundConfiguration = new ReuploadBackgroundConfiguration();
            configuration(reuploadBackgroundConfiguration);

            return services
                .AddScoped<ReuploadBackgroundConfiguration>()
                .AddHostedService<ReuploadBackgroundService>();
        }
    }
}