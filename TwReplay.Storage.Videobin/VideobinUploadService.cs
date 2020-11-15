using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TwReplay.Storage.Abstraction;

namespace TwReplay.Storage.Videobin
{
    public class VideobinUploadService : IUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly VideobinUploadServiceConfig _videobinUploadServiceConfig;

        public VideobinUploadService(HttpClient httpClient,
            VideobinUploadServiceConfig videobinUploadServiceConfig)
        {
            _httpClient = httpClient;
            _videobinUploadServiceConfig = videobinUploadServiceConfig;
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

                if (!Uri.IsWellFormedUriString(videoUrl, UriKind.Absolute))
                {
                    return new UploadPayload(false, default, null);
                }
                
                return new UploadPayload(true, videoUrl, default);
            }
            catch (Exception ex)
            {
                return new UploadPayload(false, default, ex);
            }
        }
    }
}