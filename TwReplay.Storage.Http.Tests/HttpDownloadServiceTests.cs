using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
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
            _httpDownloadService = new HttpDownloadService(httpClient);
        }

        [Test]
        public async Task SimpleVideoDownloadTest()
        {
            var progressManager = new ProgressManager<ProgressEvent>();

            var downloadVideo = await _httpDownloadService.Download(
                "https://file-examples-com.github.io/uploads/2017/04/file_example_MP4_480_1_5MG.mp4", progressManager);

            Assert.True(downloadVideo.Succeed);
            Assert.AreEqual(1570024, downloadVideo.File.Length);
        }
    }
}