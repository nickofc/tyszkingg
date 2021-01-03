namespace TwReplay.Storage.Abstraction
{
    public class RemoteFileInfo
    {
        public RemoteFileInfo(bool exists, string rawUrl, string url)
        {
            Exists = exists;
            RawUrl = rawUrl;
            Url = url;
        }

        public bool Exists { get; }
        public string RawUrl { get; }
        public string Url { get; }
    }
}