using System.Threading.Tasks;
using TwReplay.Twitch.Abstraction;

namespace TwReplay.Twitch
{
    public class TwitchClipDownloader
    {
        public async Task<string> GetDownloadUrl(Clip clip)
        {
            return clip.GetVideoRawUrl();
        }
    }
}