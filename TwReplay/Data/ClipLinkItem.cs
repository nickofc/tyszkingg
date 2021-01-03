using System;

namespace TwReplay.Data
{
    public class ClipLinkItem : Entity
    {
        public string ProviderType { get; set; }
        public string Url { get; set; }

        public int ClipItemId { get; set; }
        public ClipItem ClipItem { get; set; }

        public Availability Availability { get; set; }
        public DateTimeOffset? CheckedAt { get; set; }
    }
}