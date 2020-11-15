using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TwReplay.Twitch.Abstraction
{
    public class Clip
    {
        [JsonProperty("slug")]
        public string Slug { get; set; } 

        [JsonProperty("tracking_id")]
        public string TrackingId { get; set; } 

        [JsonProperty("url")]
        public string Url { get; set; } 

        [JsonProperty("embed_url")]
        public string EmbedUrl { get; set; } 

        [JsonProperty("embed_html")]
        public string EmbedHtml { get; set; } 

        [JsonProperty("broadcaster")]
        public Broadcaster Broadcaster { get; set; } 

        [JsonProperty("curator")]
        public Curator Curator { get; set; } 

        [JsonProperty("vod")]
        public object Vod { get; set; } 

        [JsonProperty("broadcast_id")]
        public string BroadcastId { get; set; } 

        [JsonProperty("game")]
        public string Game { get; set; } 

        [JsonProperty("language")]
        public string Language { get; set; } 

        [JsonProperty("title")]
        public string Title { get; set; } 

        [JsonProperty("views")]
        public int Views { get; set; } 

        [JsonProperty("duration")]
        public double Duration { get; set; } 

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; } 

        [JsonProperty("thumbnails")]
        public Thumbnails Thumbnails { get; set; } 
        
        public string GetVideoRawUrl()
        {
            const string pattern = @"twitch\.tv\/(.+)-(preview)";
            var regex = new Regex(pattern);

            var url = Thumbnails.Tiny;
            var id = regex.Match(url).Groups[1];

            return $"https://clips-media-assets2.twitch.tv/{id.Value}.mp4";
        }
    }
}