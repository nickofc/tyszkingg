using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitchLib.Api.V5.Models.Clips;

// ReSharper disable once CheckNamespace
namespace TwitchLib.Api.V5
{
    public static class ClipExtensions
    {
        public static string GetSlug(this Clip clip)
        {
            // https://clips.twitch.tv/CheerfulEncouragingPelicanOMGScoots?tt_medium=clips_api&tt_content=url
            const string prefix = "https://clips.twitch.tv/";
            const string suffix = "?";

            var slug = clip.Url.Replace(prefix, string.Empty, StringComparison.Ordinal);
            return slug.Substring(0, slug.IndexOf(suffix, StringComparison.Ordinal));
        }

        public static string GetClipRawUrl(this Clip clip)
        {
            const string pattern = @"twitch\.tv\/(.+)-(preview)";
            var regex = new Regex(pattern);

            var tinyUrl = clip.Thumbnails.Tiny;
            var id = regex.Match(tinyUrl).Groups[1];

            return $"https://clips-media-assets2.twitch.tv/{id.Value}.mp4";
        }

        public static async Task<TopClipsResponse> GetClipsAsync(this Clips api,
            string channel, int requiredClipCount, string cursor = null,
            string game = null, Period period = Period.All)
        {
            var clips = new List<Clip>();
            var clipsToAdd = requiredClipCount;

            do
            {
                var roundTripMaxClips = Math.Min(clipsToAdd, 100);
                clipsToAdd -= roundTripMaxClips;

                var clipsResponse = await api.GetTopClipsAsync(channel,
                    cursor, game, roundTripMaxClips, period);

                cursor = clipsResponse.Cursor;
                clips.AddRange(clipsResponse.Clips);
            } while (!string.IsNullOrEmpty(cursor) && clips.Count < requiredClipCount);

            return new TopClipsResponseCustom(clips, cursor);
        }

        private class TopClipsResponseCustom : TopClipsResponse
        {
            public TopClipsResponseCustom(List<Clip> clips, string cursor) : base(default)
            {
                Clips = clips ?? throw new ArgumentNullException(nameof(clips));
                Cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            }
        }
    }
}