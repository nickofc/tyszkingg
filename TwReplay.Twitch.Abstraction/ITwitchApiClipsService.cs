using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwReplay.Twitch.Abstraction
{
    public interface ITwitchApiClipsService
    {
        Task<IReadOnlyCollection<Clip>> GetClips(string channelName, int limit);
        Task<Clip> GetClip(string slug);
    }
}