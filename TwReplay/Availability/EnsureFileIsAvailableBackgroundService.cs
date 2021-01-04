using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TwReplay.Services
{
    public class EnsureFileIsAvailableBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EnsureFileIsAvailableBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // // while (true)
            // // {
            // //     using (var provider = _serviceScopeFactory.CreateScope())
            // //     {
            // //         var uploadService = provider.ServiceProvider.GetRequiredService<IUploadService>();
            // //         var dbContext = provider.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // //         var twitchClipService = provider.ServiceProvider.GetRequiredService<ITwitchApiClipsService>();
            // //         var logger = provider.ServiceProvider
            // //             .GetRequiredService<ILogger<EnsureFileIsAvailableBackgroundService>>();
            // //
            // //         // TODO: take clips in parts (ex. 100 items)
            // //         // it will use much less memory. 
            // //
            // //         var videos = await dbContext.Clips
            // //             .Include(item => item.ClipInfo)
            // //             .Include(item => item.Links)
            // //             .ToArrayAsync(cancellationToken: stoppingToken);
            // //
            // //         foreach (var video in videos)
            // //         {
            // //             logger.LogInformation($"Checking video (id: {video.Id}) (slug: {video.ClipInfo.Slug}).");
            // //
            // //             var links = video.Links
            // //                 .Where(x => x.Provider == uploadService.GetType().Name)
            // //                 .ToArray();
            // //
            // //             if (!links.Any())
            // //             {
            // //                 logger.LogInformation(
            // //                     $"Links for video (id: {video.Id} slug: {video.ClipInfo.Slug}) with current provider (type: {uploadService.GetType().Name}) do not exist.");
            // //             }
            // //
            // //             foreach (var link in links)
            // //             {
            // //                 logger.LogInformation(
            // //                     $"Checking link (url: {link.Url}) for video (id: {video.Id} slug: {video.ClipInfo.Slug}) with provider (type: {uploadService.GetType().Name}).");
            // //
            // //                 var remoteFileInfo = await uploadService.GetRemoteFileInfo(link.Url);
            // //                 if (!remoteFileInfo.Exists)
            // //                 {
            // //                     logger.LogWarning(
            // //                         $"Link for video (id: {video.Id} slug: {video.ClipInfo.Slug}) (url: {link.Url}) is not available.");
            // //
            // //                     var clip = await twitchClipService.GetClip(video.ClipInfo.Slug);
            // //
            // //                     if (clip == null)
            // //                     {
            // //                         logger.LogWarning(
            // //                             $"Video (id: {video.Id} slug: {video.ClipInfo.Slug}) (url: {link.Url}) is not available.");
            // //
            // //                         video.ClipInfo.IsAvailable = false;
            // //                     }
            // //
            // //                     link.IsAvailable = false;
            // //
            // //                     dbContext.Update(video);
            // //                     await dbContext.SaveChangesAsync(stoppingToken);
            // //                 }
            // //
            // //                 logger.LogInformation(
            // //                     $"Link for video (id: {video.Id} slug: {video.ClipInfo.Slug}) (url: {link.Url}) is available.");
            // //             }
            // //         }
            // //     }
            // //
            // //     stoppingToken.ThrowIfCancellationRequested();
            // //     await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            // //}
        }
    }
}