using System.IO;
using System.Threading.Tasks;

namespace TwReplay.Twitch
{
    public class TwitchClipPayload
    {
        public TwitchClipPayload(bool downloaded, byte[] video, string slug)
        {
            Downloaded = downloaded;
            Video = video;
            Slug = slug;
        }

        public bool Downloaded { get; }
        public byte[] Video { get; }
        public string Slug { get; }

        public Task Save(string dir)
        {
            var url = Path.Combine(dir, $"{Slug ?? Path.GetFileName(Path.GetTempFileName())}.mp4");
            return File.WriteAllBytesAsync(url, Video);
        }
    }
}