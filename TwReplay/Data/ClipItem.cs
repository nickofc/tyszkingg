using System;
using System.Collections.Generic;

namespace TwReplay.Data
{
    public class ClipItem
    {
        public string Id { get; set; }
        public ClipInfo ClipInfo { get; set; }
        public List<ClipLinkInfo> Links { get; set; }
        public DateTimeOffset? AddedAt { get; set; }
        public ClipItem()
        {
            ClipInfo = new ClipInfo();
            Links = new List<ClipLinkInfo>();
        }
    }
}