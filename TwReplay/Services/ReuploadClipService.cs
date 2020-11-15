using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TwReplay.Storage.Abstraction;
using TwReplay.Twitch;
using TwReplay.Twitch.Abstraction;

namespace TwReplay.Services
{
    public class ReuploadClipService
    {
        private readonly TwitchClipDownloader _clipDownloader;
        private readonly IDownloadService _downloadService;
        private readonly IUploadService _uploadService;

        public ReuploadClipService(TwitchClipDownloader clipDownloader, 
            IDownloadService downloadService, IUploadService uploadService)
        {
            _clipDownloader = clipDownloader;
            _downloadService = downloadService;
            _uploadService = uploadService;
        }

        public async Task<ReuplaodPayload> Reupload(Clip clip, 
            CancellationToken cancellationToken = default)
        {
            var progressManager = new ProgressManager<ProgressEvent>();
            
            var downloadUrl = await _clipDownloader.GetDownloadUrl(clip);
            var downloadPayload = await _downloadService.Download(downloadUrl,
                progressManager, cancellationToken);

            if (!downloadPayload.Succeed)
            {
                return new ReuplaodPayload(ReuplaodPayload.ReuploadStatus.DownloadFailed);
            }

            var memoryStream = new MemoryStream(downloadPayload.File);
            var uploadPayload = await _uploadService.Upload(memoryStream, 
                progressManager, cancellationToken);

            if (!uploadPayload.Succeed)
            {
                return new ReuplaodPayload(ReuplaodPayload.ReuploadStatus.UploadFailed);
            }
            
            return new ReuplaodPayload(ReuplaodPayload.ReuploadStatus.Ok, 
                uploadPayload.Url, _uploadService.GetType().Name);
        }

        public class ReuplaodPayload
        {
            public ReuplaodPayload(ReuploadStatus status, string url, string provider)
            {
                Status = status;
                Url = url;
                Provider = provider;
            }

            public ReuploadStatus Status { get; }

            public string Url { get; set; }
            public string Provider { get; set; }
            public ReuplaodPayload(ReuploadStatus status)
            {
                Status = status;
            }

            public enum ReuploadStatus
            {
                DownloadFailed,
                UploadFailed,
                Ok,
            }
        }
    }
}