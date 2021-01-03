using System;
using Microsoft.Extensions.DependencyInjection;
using TwReplay.Services;
using TwReplay.TTV;

// ReSharper disable once CheckNamespace
namespace TwitchLib.Api
{
    // ReSharper disable once InconsistentNaming
    public static class TwitchAPIServiceCollectionExtensions
    {
        public static IServiceCollection AddTwitchApi(this IServiceCollection serviceCollection,
            Action<TwitchApiOptions> configFunc)
        {
            var config = new TwitchApiOptions();
            configFunc(config);
            return serviceCollection.AddTwitchApi(config);
        }

        public static IServiceCollection AddTwitchApi(this IServiceCollection serviceCollection,
            TwitchApiOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return serviceCollection.AddScoped(_ =>
            {
                return new TwitchAPI
                {
                    Settings =
                    {
                        ClientId = options.ClientId,
                        Secret = options.ClientSecret
                    }
                };
            }).AddScoped<TwitchClipService>()
              .AddScoped<TwitchClipDownloader>();
        }
    }
}