using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TwReplay.Services.Download;
using TwReplay.Storage.Abstraction;

namespace TwReplay.Storage.Http.Tests
{
    public class HttpDownloadServiceTests
    {
        private HttpDownloadService _httpDownloadService;

        [SetUp]
        public void SetUp()
        {
            var httpClient = new HttpClient();
            _httpDownloadService = new HttpDownloadService(httpClient,
                new Logger<HttpDownloadService>(new NullLoggerFactory()));
        }

        [Test]
        public async Task When_StatusCodeIsOkay_Then_DownloadFile()
        {
            const string validUrl =
                "https://file-examples-com.github.io/uploads/2017/04/file_example_MP4_480_1_5MG.mp4";

            var progressManager = new ProgressManager<ProgressEvent>();
            var downloadVideo = await _httpDownloadService.Download(validUrl, progressManager);

            Assert.True(downloadVideo.Succeed);
            Assert.AreEqual(1570024, downloadVideo.File.Length);
        }

        [Test]
        public async Task When_StatusCodeEqualsNotFound_Then_ThrowException()
        {
            const string invalidUrl =
                "https://file-examples-com.github.io/uploads/2017/04/file_example_MP4_480_1_5MG.mp45";

            var progressManager = new ProgressManager<ProgressEvent>();
            var downloadVideo = await _httpDownloadService.Download(invalidUrl, progressManager);

            Assert.False(downloadVideo.Succeed);
        }
    }
}