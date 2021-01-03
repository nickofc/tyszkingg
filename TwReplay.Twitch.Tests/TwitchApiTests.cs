using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using TwReplay.Twitch.Apis;

namespace TwReplay.Twitch.Tests
{
    public class TwitchApiTests
    {
        private TwitchApi _twitchApi;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            var twitchApiConfig = new TwitchApiConfig
            {
                ClientId = "3cqqaf7hh7ywkvypgk1f1pyorvh4uj",
                ClientSecret = "y0eg5n0xzhnd7pw1rfztpnk3oq72l3"
            };

            _twitchApi = new TwitchApi(httpClient, twitchApiConfig);
        }

        [Test]
        public async Task GetClipsTests()
        {
            var tokenPayload = await _twitchApi.GetToken();
            _twitchApi.SetAccessToken(tokenPayload.AccessToken);

            var twitchClipsService = new TwitchApiClipsService(_twitchApi);
            var clips = await twitchClipsService.GetClips("tyszkingg", 100);

            Assert.NotNull(clips);
            Assert.True(clips.Count > 0);
        }

        [Test]
        public async Task GetClipBySlug()
        {
            var tokenPayload = await _twitchApi.GetToken();
            _twitchApi.SetAccessToken(tokenPayload.AccessToken);

            var twitchClipsService = new TwitchApiClipsService(_twitchApi);

            var clip = await twitchClipsService.GetClip("DeliciousAbrasiveSandwichCopyThis");
            Assert.IsNotNull(clip);
        }

        [Test]
        public async Task GetClipBySlug_NotFound()
        {
            var tokenPayload = await _twitchApi.GetToken();
            _twitchApi.SetAccessToken(tokenPayload.AccessToken);

            var twitchClipsService = new TwitchApiClipsService(_twitchApi);

            var clip = await twitchClipsService.GetClip("DeliciousAbrasiveSandwichCopyThis1");
            Assert.IsNull(clip);
        }
    }
}