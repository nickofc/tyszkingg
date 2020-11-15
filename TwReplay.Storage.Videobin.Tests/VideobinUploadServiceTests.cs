using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
                new VideobinUploadServiceConfig());
        }

        [Test]
        public async Task Test1()
        {
            var fs = File.OpenRead("sample.mp4");
            var uploadPayload = await _videobinUploadService.Upload(fs,
                new ProgressManager<ProgressEvent>(), CancellationToken.None);
            
            Assert.True(uploadPayload.Succeed);
            Assert.IsNotEmpty(uploadPayload.Url);
            Assert.IsNull(uploadPayload.Exception);
        }
    }
}