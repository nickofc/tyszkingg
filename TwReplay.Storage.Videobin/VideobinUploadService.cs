using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TwReplay.Storage.Abstraction;

namespace TwReplay.Storage.Videobin
{
    public class VideobinUploadService : IUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public VideobinUploadService(HttpClient httpClient,
            ILogger<VideobinUploadService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public bool FileUrlIsValid(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return false;

            return string.Equals(uri.Host, "videobin.org", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<RemoteFileInfo> GetRemoteFileInfo(string url)
        {
            if (!FileUrlIsValid(url))
                return new RemoteFileInfo(false, default, url);

            if (!url.EndsWith(".html"))
                return new RemoteFileInfo(false, default, url);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new RemoteFileInfo(false, null, url);

            var rawUrl = url.Replace(".html", ".ogg");
            return new RemoteFileInfo(true, rawUrl, url);
        }

        public async Task<UploadPayload> Upload(Stream stream,
            ProgressManager<ProgressEvent> progressManager,
            CancellationToken cancellationToken = default)
        {
            var destination = new MemoryStream();
            await stream.CopyToAsync(destination, cancellationToken);

            var formContent = new MultipartFormDataContent
            {
                {new StringContent("1"), "api"},
                {new ByteArrayContent(destination.ToArray()), "videoFile", "file"}
            };

            try
            {
                var response = await _httpClient.PostAsync(
                    "https://videobin.org/add", formContent, cancellationToken);
                response.EnsureSuccessStatusCode();

                var videoUrl = await response.Content
                    .ReadAsStringAsync(cancellationToken);

                _logger.LogTrace($"File uploaded. Body from VideoBin service: {videoUrl}.");

                if (!Uri.IsWellFormedUriString(videoUrl, UriKind.Absolute))
                {
                    _logger.LogError("Uploaded video url is not valid url.");
                    return new UploadPayload(false, default, null);
                }

                return new UploadPayload(true, videoUrl, default);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to upload file.");
                return new UploadPayload(false, default, ex);
            }
        }
    }
}