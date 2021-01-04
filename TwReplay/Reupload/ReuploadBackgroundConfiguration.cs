using System;

namespace TwReplay.Services
{
    public class ReuploadBackgroundConfiguration
    {
        public TimeSpan Delay { get; set; } = TimeSpan.FromMinutes(1);
    }
}