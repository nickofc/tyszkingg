using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.V5;
using TwitchLib.Api.V5.Models.Clips;
using TwReplay.Services;
using TwReplay.Storage.Abstraction;

namespace TwReplay.TTV
{
    public class TwitchClipService
    {
        private readonly TwitchAPI _twitchApi;
        private readonly IDownloadService _downloadService;
        private readonly TwitchClipDownloader _twitchClipDownloader;

        public TwitchClipService(TwitchAPI twitchApi, IDownloadService downloadService,
            TwitchClipDownloader twitchClipDownloader)
        {
            _twitchApi = twitchApi;
            _downloadService = downloadService;
            _twitchClipDownloader = twitchClipDownloader;
        }

        public async Task<Clip[]> GetClips(string channelName)
        {
            var topClips = await _twitchApi.V5.Clips
                .GetClipsAsync(channelName, int.MaxValue);
            return topClips.Clips.ToArray();
        }

        public async Task<DownloadClipPayload> DownloadClip(Clip clip,
            ProgressManager<ProgressEvent> progressManager,
            CancellationToken cancellationToken)
        {
            var clipDownloadUrl = await _twitchClipDownloader.GetDownloadUrl(clip);
            var download = await _downloadService.Download(clipDownloadUrl,
                progressManager, cancellationToken);

            return new DownloadClipPayload(download.Succeed, download.File);
        }
    }
}