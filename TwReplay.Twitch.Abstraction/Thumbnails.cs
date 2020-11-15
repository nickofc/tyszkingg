using Newtonsoft.Json;

namespace TwReplay.Twitch.Abstraction
{
    public class Thumbnails
    {
        [JsonProperty("medium")]
        public string Medium { get; set; } 

        [JsonProperty("small")]
        public string Small { get; set; } 

        [JsonProperty("tiny")]
        public string Tiny { get; set; } 
    }
}