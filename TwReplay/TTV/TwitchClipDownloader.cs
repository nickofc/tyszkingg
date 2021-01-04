using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api.V5;
using TwitchLib.Api.V5.Models.Clips;
using TwReplay.Services;
using TwReplay.Storage.Abstraction;

namespace TwReplay.TTV
{
    public class TwitchClipDownloader
    {
        private readonly IDownloadService _downloadService;

        public TwitchClipDownloader(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        public async Task<DownloadClipPayload> DownloadClip(Clip clip,
            ProgressManager<ProgressEvent> progressManager,
            CancellationToken cancellationToken)
        {
            var clipDownloadUrl = await GetDownloadUrl(clip);
            var download = await _downloadService.Download(clipDownloadUrl,
                progressManager, cancellationToken);

            return new DownloadClipPayload(download.Succeed, download.File);
        }

        public async Task<string> GetDownloadUrl(Clip clip)
        {
            return clip.GetClipRawUrl();
        }
    }
}