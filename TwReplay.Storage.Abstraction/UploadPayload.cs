using System;

namespace TwReplay.Storage.Abstraction
{
    public class UploadPayload
    {
        public UploadPayload(bool succeed, 
            string url, Exception exception)
        {
            Succeed = succeed;
            Url = url;
            Exception = exception;
        }

        public bool Succeed { get; }
        public string Url { get; }
        public Exception Exception { get; }
    }
}