using System.Threading.Tasks;
using TwitchLib.Api.V5;
using TwitchLib.Api.V5.Models.Clips;

namespace TwReplay.TTV
{
    public class TwitchClipDownloader
    {
        public async Task<string> GetDownloadUrl(Clip clip)
        {
            return clip.GetClipRawUrl();
        }
    }
}