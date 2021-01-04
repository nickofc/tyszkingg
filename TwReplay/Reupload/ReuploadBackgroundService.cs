using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitchLib.Api.V5;
using TwReplay.Data;
using TwReplay.Reupload.Services;
using TwReplay.TTV;

namespace TwReplay.Services
{
    public class ReuploadBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ReuploadBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                ReuploadBackgroundConfiguration reuploadBackgroundConfiguration;

                using (var provider = _serviceScopeFactory.CreateScope())
                {
                    var twitchClipService = provider.ServiceProvider.GetRequiredService<TwitchClipService>();
                    var reuploadService = provider.ServiceProvider.GetRequiredService<ReuploadService>();
                    var dbContext = provider.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var appConfiguration = provider.ServiceProvider.GetRequiredService<IConfiguration>();

                    reuploadBackgroundConfiguration = provider.ServiceProvider
                        .GetRequiredService<ReuploadBackgroundConfiguration>();

                    var slugs = await dbContext.ClipItems
                        .Include(x => x.ClipLinkItem)
                        .Where(x => x.ClipLinkItem.Availability == Availability.Available)
                        .Select(x => x.Slug)
                        .ToArrayAsync(stoppingToken);

                    var clips = await twitchClipService.GetClips(appConfiguration["App:Channel"]);
                    clips = clips.Where(x => !slugs.Contains(x.GetSlug())).ToArray();

                    await reuploadService.Reupload(clips, stoppingToken);
                }

                stoppingToken.ThrowIfCancellationRequested();
                await Task.Delay(reuploadBackgroundConfiguration.Delay, stoppingToken);
            }
        }
    }
}