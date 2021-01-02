using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TwReplay.Storage.Abstraction
{
    public interface IUploadService
    {
        public bool RawUrlSupport { get; }
        Task<string> GetRawUrl(string url);
        Task<bool> IsFileAvailable(string url);

        Task<UploadPayload> Upload(Stream stream,
            ProgressManager<ProgressEvent> progressManager,
            CancellationToken cancellationToken = default);
    }
}