using System;

namespace TwReplay.Storage.Abstraction
{
    public class DownloadPayload
    {
        public DownloadPayload(bool succeed, 
            byte[] file, Exception exception)
        {
            Succeed = succeed;
            File = file;
            Exception = exception;
        }
        
        public bool Succeed { get; }
        public byte[] File { get; }
        public Exception Exception { get; }
    }
}