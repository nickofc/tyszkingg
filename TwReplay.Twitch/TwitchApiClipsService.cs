using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TwReplay.Twitch.Abstraction;

namespace TwReplay.Twitch
{
    public class TwitchApiClipsService : ITwitchApiClipsService
    {
        private readonly TwitchApi _twitchApi;

        public TwitchApiClipsService(TwitchApi twitchApi)
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

        public async Task<Clip> GetClip(string slug)
        {
            try
            {
                return await _twitchApi.Get<Clip>($"https://api.twitch.tv/kraken/clips/{slug}");
            }
            catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}