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
        public static IServiceCollection AddTwitchApi(this IServiceCollection services,
            Action<TwitchApiOptions> configure)
        {
            if (configure == null) 
                throw new ArgumentNullException(nameof(configure));
            
            var twitchApiOptions = new TwitchApiOptions();
            configure(twitchApiOptions);
            
            return services.AddTwitchApi(twitchApiOptions);
        }

        public static IServiceCollection AddTwitchApi(this IServiceCollection services,
            TwitchApiOptions twitchApiOptions)
        {
            if (twitchApiOptions == null)
                throw new ArgumentNullException(nameof(twitchApiOptions));

            return services.AddScoped(_ =>
                {
                    return new TwitchAPI
                    {
                        Settings =
                        {
                            ClientId = twitchApiOptions.ClientId,
                            AccessToken = twitchApiOptions.AccessToken
                        }
                    };
                }).AddScoped<TwitchClipService>()
                .AddScoped<TwitchClipDownloader>();
        }
    }
}