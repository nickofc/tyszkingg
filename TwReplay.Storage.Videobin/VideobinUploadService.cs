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

        public async Task<bool> IsFileAvailable(string url)
        {
            var frameUrl = url.Replace(".html", ".iframe.html");

            var body = await _httpClient.GetStringAsync(frameUrl);

            var title = body.Substring(0, body.IndexOf("</title>", StringComparison.Ordinal))
                .Substring(body.IndexOf("<title>", StringComparison.Ordinal), body.Length);

            return false;
        }

        public bool RawUrlSupport { get; } = true;

        public async Task<string> GetRawUrl(string url)
        {
            if (!url.EndsWith(".html"))
            {
                throw new NotSupportedException("Url is not valid.");
            }

            return url.Replace(".html", ".ogg");
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

                _logger.LogInformation($"File uploaded. Body from VideoBin service: {videoUrl}.");

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