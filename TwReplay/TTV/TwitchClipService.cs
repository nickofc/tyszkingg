using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.V5;
using TwitchLib.Api.V5.Models.Clips;
using TwReplay.Services;
using TwReplay.Storage.Abstraction;

namespace TwReplay.TTV
{
    public class TwitchClipService
    {
        private readonly TwitchAPI _twitchApi;

        public TwitchClipService(TwitchAPI twitchApi)
        {
            _twitchApi = twitchApi;
        }

        public async Task<Clip[]> GetClips(string channelName)
        {
            var topClips = await _twitchApi.V5.Clips
                .GetClipsAsync(channelName, int.MaxValue);
            return topClips.Clips.ToArray();
        }
    }
}