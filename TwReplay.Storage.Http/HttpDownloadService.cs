using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TwReplay.Storage.Abstraction;

namespace TwReplay.Services.Download
{
    public class HttpDownloadService : IDownloadService
    {
        public HttpDownloadService(HttpClient httpClient,
            ILogger<HttpDownloadService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpDownloadService> _logger;

        public async Task<DownloadPayload> Download(string url,
            ProgressManager<ProgressEvent> progressManager,
            CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"Downloading the file {url}");

            var destination = new MemoryStream();

            HttpRequestMessage request = null;
            HttpResponseMessage response = null;

            Stream source = null;
            ProgressEvent progressEvent = new ProgressEvent();

            try
            {
                request = new HttpRequestMessage(HttpMethod.Get, url);
                response = await _httpClient.SendAsync(request, cancellationToken);

                _logger.LogTrace($"Reading the stream from: {url} ({response.StatusCode}).");

                response.EnsureSuccessStatusCode();
                source = await response.Content.ReadAsStreamAsync(cancellationToken);

                var buffer = new byte[1024 * 10];
                var totalSent = 0;
                int count;

                progressEvent.Succeed = false;
                progressEvent.Completed = false;
                progressEvent.Current = 0;
                progressEvent.Total = response.Content.Headers
                    .ContentLength.GetValueOrDefault(-1);

                progressManager.Report(progressEvent);

                while ((count = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destination.Write(buffer, 0, count);
                    totalSent += count;

                    progressEvent.Current = totalSent;
                    progressManager.Report(progressEvent);
                }

                _logger.LogTrace($"Reading stream from {url} completed.");

                progressEvent.Completed = true;
                progressEvent.Succeed = true;

                progressManager.Report(progressEvent);


                return new DownloadPayload(true,
                    destination.ToArray(), default);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to download file: {url}.");

                progressEvent.Completed = true;
                progressEvent.Succeed = false;

                progressManager.Report(progressEvent);

                return new DownloadPayload(
                    false, default, ex);
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();

                source?.Dispose();
                destination?.Dispose();
            }
        }
    }
}