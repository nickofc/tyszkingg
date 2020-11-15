using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwReplay.Twitch
{
    public class TwitchApi
    {
        private readonly HttpClient _httpClient;
        private readonly TwitchApiConfig _twitchApiConfig;

        public TwitchApi(HttpClient httpClient,
            TwitchApiConfig twitchApiConfig)
        {
            _httpClient = httpClient;
            _twitchApiConfig = twitchApiConfig;
            
            _httpClient = new HttpClient();
            _twitchApiConfig = new TwitchApiConfig
            {
                ClientId = "3cqqaf7hh7ywkvypgk1f1pyorvh4uj",
                ClientSecret = "y0eg5n0xzhnd7pw1rfztpnk3oq72l3"
            };

            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/vnd.twitchtv.v5+json");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Client-ID", _twitchApiConfig.ClientId);
        }
        
        public void SetAccessToken(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "Authorization", $"Bearer {accessToken}");
        }

        public void RemoveAccessToken()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }

        public async Task<T> Get<T>(string url)
        {
            var httpResponseMessage = await _httpClient.GetAsync(url);
            httpResponseMessage.EnsureSuccessStatusCode();

            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T> Post<T>(string url)
        {
            var httpResponseMessage = await _httpClient.PostAsync(url, new ReadOnlyMemoryContent(ReadOnlyMemory<byte>.Empty));
            httpResponseMessage.EnsureSuccessStatusCode();

            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
        
        public Task<TokenPayload> GetToken()
        {
            return Post<TokenPayload>($"https://id.twitch.tv/oauth2/token?client_id={_twitchApiConfig.ClientId}&client_secret={_twitchApiConfig.ClientSecret}&grant_type=client_credentials");
        }
    }
}