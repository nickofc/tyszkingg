namespace TwReplay.Data
{
    public class ClipLinkInfo
    {
        public string Id { get; set; }
        public string Provider { get; set; }
        public string Url { get; set; }
        public string ClipItemId { get; set; }
        public ClipItem ClipItem { get; set; }
        public bool IsAvailable { get; set; }
    }
}