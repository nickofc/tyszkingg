using Newtonsoft.Json;

namespace TwReplay.Twitch.Abstraction
{
    public class Curator
    {
        [JsonProperty("id")]
        public string Id { get; set; } 

        [JsonProperty("name")]
        public string Name { get; set; } 

        [JsonProperty("display_name")]
        public string DisplayName { get; set; } 

        [JsonProperty("channel_url")]
        public string ChannelUrl { get; set; } 

        [JsonProperty("logo")]
        public string Logo { get; set; } 
    }
}