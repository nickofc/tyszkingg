using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TwReplay.Storage.Abstraction;
using TwReplay.Twitch;
using TwReplay.Twitch.Abstraction;

namespace TwReplay.Services
{
    public class TwitchClipService
    {
        private readonly TwitchApi _twitchApi;
        private readonly ITwitchApiClipsService _twitchApiClipsService;
        private readonly IMemoryCache _memoryCache;
        private readonly TwitchClipDownloader _twitchClipDownloader;
        private readonly IDownloadService _downloadService;

        public TwitchClipService(TwitchApi twitchApi, ITwitchApiClipsService twitchApiClipsService,
            IMemoryCache memoryCache, TwitchClipDownloader twitchClipDownloader, IDownloadService downloadService)
        {
            _twitchApi = twitchApi;
            _twitchApiClipsService = twitchApiClipsService;
            _memoryCache = memoryCache;
            _twitchClipDownloader = twitchClipDownloader;
            _downloadService = downloadService;
        }

        public async Task<IReadOnlyCollection<Clip>> GetClips(string channelName)
        {
            if (!_memoryCache.TryGetValue(channelName,
                out IReadOnlyCollection<Clip> clips))
            {
                var tokenPayload = await _twitchApi.GetToken();
                _twitchApi.SetAccessToken(tokenPayload.AccessToken);

                clips = await _twitchApiClipsService.GetClips(channelName, 100);
                _memoryCache.Set(channelName, clips, TimeSpan.FromMinutes(1));
            }

            return clips;
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