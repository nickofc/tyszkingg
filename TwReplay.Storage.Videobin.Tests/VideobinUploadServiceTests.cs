using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TwReplay.Storage.Abstraction;

namespace TwReplay.Storage.Videobin.Tests
{
    public class VideobinUploadServiceTests
    {
        private VideobinUploadService _videobinUploadService;

        [SetUp]
        public void Setup()
        {
            _videobinUploadService = new VideobinUploadService(new HttpClient(),
                new Logger<VideobinUploadService>(new NullLoggerFactory()));
        }

        [Test]
        public async Task UploadTest()
        {
            var fs = File.OpenRead("sample.mp4");
            var uploadPayload = await _videobinUploadService.Upload(fs,
                new ProgressManager<ProgressEvent>(), CancellationToken.None);

            Assert.True(uploadPayload.Succeed);
            Assert.IsNotEmpty(uploadPayload.Url);
            Assert.IsNull(uploadPayload.Exception);

            await fs.DisposeAsync();
        }

        [Test]
        public async Task GetRawUrlTest()
        {
            const string url = "https://videobin.org/+1dr7/1kcv.html";
            var rawUrl = await _videobinUploadService.GetRawUrl(url);

            Assert.AreEqual("https://videobin.org/+1dr7/1kcv.ogg", rawUrl);
        }

        [Test]
        public async Task IsFileAvailableTest()
        {
            const string url = "https://videobin.org/+1dqy/1kcm.html";
            var isFileAvailable = await _videobinUploadService.IsFileAvailable(url);

            Assert.False(isFileAvailable);
        }
    }
}