using System;

namespace TwReplay.Storage.Abstraction
{
    public class ProgressEvent
    {
        public bool Completed { get; set; }
        public bool? Succeed { get; set; }
        public long Current { get; set; }
        public long Total { get; set; }
        public int Percentage
        {
            get
            {
                var value = (int) Math.Round((double) (100 * Current) / Total);
                return Math.Min(Math.Max(value, 0), 100);
            }
        }
    }
}