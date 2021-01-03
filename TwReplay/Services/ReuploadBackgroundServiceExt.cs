using System;
using Microsoft.Extensions.DependencyInjection;

namespace TwReplay.Services
{
    public static class ReuploadBackgroundServiceExt
    {
        public static IServiceCollection AddReuploadBackgroundService(IServiceCollection serviceCollection,
            Action<ReuploadBackgroundConfiguration> configuration)
        {
            var reuploadBackgroundConfiguration = new ReuploadBackgroundConfiguration();
            configuration(reuploadBackgroundConfiguration);

            return serviceCollection
                .AddScoped<ReuploadBackgroundConfiguration>()
                .AddHostedService<ReuploadBackgroundService>();
        }
    }
}