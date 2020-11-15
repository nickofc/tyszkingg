using System.Collections.Generic;
using TwReplay.Twitch.Abstraction;

namespace TwReplay.Twitch
{
    public class GetClipsPayload
    {
        public IList<Clip> Clips { get; set; }
        public string Cursor { get; set; }
    }
}