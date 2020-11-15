using System.Threading;
using System.Threading.Tasks;

namespace TwReplay.Storage.Abstraction
{
    public interface IDownloadService
    {
        Task<DownloadPayload> Download(string url,
            ProgressManager<ProgressEvent> progressManager,
            CancellationToken cancellationToken = default);
    }
}