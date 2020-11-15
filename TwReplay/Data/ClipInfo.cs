using System;

namespace TwReplay.Data
{
    public class ClipInfo
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Game { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ClipItemId { get; set; }
        public ClipItem ClipItem { get; set; }
    }
}