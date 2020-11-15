namespace TwReplay.Services
{
    public class DownloadClipPayload
    {
        public bool Succeed { get; }
        public byte[] File { get; }

        public DownloadClipPayload(bool succeed, byte[] file)
        {
            Succeed = succeed;
            File = file;
        }
    }
}