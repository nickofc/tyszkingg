using System;
using System.Collections.Generic;

namespace TwReplay.Data
{
    public class ClipItem
    {
        public Guid Id { get; set; }
        
        public string Slug { get; set; }
        public string Name { get; set; }
        public List<ClipLinkInfo> Links { get; set; }
        public DateTimeOffset AddedAt { get; set; }

        public ClipItem()
        {
            Links = new List<ClipLinkInfo>();
        }
    }
}