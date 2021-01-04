using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api.V5.Models.Clips;
using TwReplay.Services;
using TwReplay.Storage.Abstraction;
using TwReplay.TTV;

namespace TwReplay.Reupload.Services
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

        public async Task<ReuploadPayload> Reupload(Clip clip,
            CancellationToken cancellationToken = default)
        {
            var progressManager = new ProgressManager<ProgressEvent>();

            var downloadUrl = await _clipDownloader.GetDownloadUrl(clip);
            var downloadPayload = await _downloadService.Download(downloadUrl,
                progressManager, cancellationToken);

            if (!downloadPayload.Succeed)
            {
                return new ReuploadPayload(ReuploadStatus.DownloadFailed);
            }

            var memoryStream = new MemoryStream(downloadPayload.File);
            var uploadPayload = await _uploadService.Upload(memoryStream,
                progressManager, cancellationToken);

            if (!uploadPayload.Succeed)
            {
                return new ReuploadPayload(ReuploadStatus.UploadFailed);
            }

            return new ReuploadPayload(ReuploadStatus.Ok,
                uploadPayload.Url, _uploadService.GetType().Name);
        }
    }
}