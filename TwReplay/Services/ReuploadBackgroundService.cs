using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwReplay.Data;

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
                using (var provider = _serviceScopeFactory.CreateScope())
                {
                    var twitchClipService = provider.ServiceProvider.GetRequiredService<TwitchClipService>();
                    var reuploadService = provider.ServiceProvider.GetRequiredService<ReuploadService>();
                    var dbContext = provider.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var slugs = await dbContext.Clips
                        .Include(x => x.ClipInfo)
                        .Select(x => x.ClipInfo.Slug)
                        .ToArrayAsync(cancellationToken: stoppingToken);

                    var clips = await twitchClipService.GetClips("tyszkingg");
                    clips = clips.Where(x => !slugs.Contains(x.Slug)).ToArray();

                    await reuploadService.Reupload(clips, stoppingToken);
                }   
                
                stoppingToken.ThrowIfCancellationRequested();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}