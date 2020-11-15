using System;

namespace TwReplay.Storage.Abstraction
{
    public class ProgressManager<T> where T : class
    {
        public event EventHandler<T> Reporeted;

        public void Report(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Reporeted?.Invoke(this, obj);
        }
    }
}