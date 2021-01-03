using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TwReplay.Storage.Abstraction
{
    public interface IUploadService
    {
        Task<RemoteFileInfo> GetRemoteFileInfo(string url);
        Task<UploadPayload> Upload(Stream stream,
            ProgressManager<ProgressEvent> progressManager,
            CancellationToken cancellationToken = default);
    }
}