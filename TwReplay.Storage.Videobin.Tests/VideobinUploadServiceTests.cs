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
        public async Task When_ProvideValidFile_Should_UploadFile()
        {
            var fs = File.OpenRead("sample.mp4");

            var uploadPayload = await _videobinUploadService.Upload(fs,
                new ProgressManager<ProgressEvent>(), CancellationToken.None);

            Assert.True(uploadPayload.Succeed);
            Assert.IsNotEmpty(uploadPayload.Url);
            Assert.IsNull(uploadPayload.Exception);
        }

        [Test]
        public async Task When_ServerReturnsNotFoundStatusCode_Should_ReturnNotAccessible()
        {
            const string urlThatNotExists = "https://videobin.org/+1dr7/1kca.html";

            var remoteFileInfo = await _videobinUploadService.GetRemoteFileInfo(urlThatNotExists);

            Assert.False(remoteFileInfo.Exists);
            Assert.Null(remoteFileInfo.RawUrl);
            Assert.AreEqual(urlThatNotExists, remoteFileInfo.Url);
        }

        [Test]
        public async Task When_ServerReturnsOkStatusCode_Should_ReturnValidInfo()
        {
            const string urlThatExists = "https://videobin.org/+1dr7/1kcv.html";

            var remoteFileInfo = await _videobinUploadService.GetRemoteFileInfo(urlThatExists);

            Assert.True(remoteFileInfo.Exists);
            Assert.AreEqual("https://videobin.org/+1dr7/1kcv.ogg", remoteFileInfo.RawUrl);
            Assert.AreEqual(urlThatExists, remoteFileInfo.Url);
        }
    }
}