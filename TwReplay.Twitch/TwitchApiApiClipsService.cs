using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwReplay.Twitch.Abstraction;

namespace TwReplay.Twitch
{
    public class TwitchApiApiClipsService : ITwitchApiClipsService
    {
        private readonly TwitchApi _twitchApi;

        public TwitchApiApiClipsService(TwitchApi twitchApi)
        {
            _twitchApi = twitchApi;
        }

        public async Task<IReadOnlyCollection<Clip>> GetClips(string channelName, int limit = 10)
        {
            var payload = await _twitchApi.Get<GetClipsPayload>(
                $"https://api.twitch.tv/kraken/clips/top?limit={limit}&channel={channelName}");

            if (payload == null ||
                payload.Clips == null)
            {
                return Array.Empty<Clip>();
            }

            return payload.Clips.ToArray();
        }
    }
}