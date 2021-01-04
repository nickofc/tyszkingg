using System;

namespace TwReplay.Data
{
    public class ClipItem : Entity
    {
        public string Slug { get; set; }
        public string Description { get; set; }

        public int ClipLinkItemId { get; set; }
        public ClipLinkItem ClipLinkItem { get; set; }

        public DateTimeOffset AddedAt { get; set; }
        public bool ClipDeleted { get; set; }
    }
}