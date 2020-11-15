using NUnit.Framework;
using TwReplay.Twitch.Abstraction;

namespace TwReplay.Twitch.Tests
{
    public class TwitchClipDownloaderTests
    {
        private TwitchClipDownloader _twitchClipDownloader;

        [SetUp]
        public void Setup()
        {
            _twitchClipDownloader = new TwitchClipDownloader();
        }

        [Test]
        public void GetVideoUrl()
        {
            var clips = new Clip
            {
                Thumbnails = new Thumbnails
                {
                    Tiny = "https://clips-media-assets2.twitch.tv/39894030060-offset-1088-preview-480x272.jpg",
                }
            };

            Assert.AreEqual("https://clips-media-assets2.twitch.tv/39894030060-offset-1088.mp4", clips.GetVideoRawUrl());
        }
    }
}