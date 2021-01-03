namespace TwReplay.Reupload
{
    public class ReuplaodPayload
    {
        public ReuplaodPayload(ReuploadStatus status, string url, string provider)
        {
            Status = status;
            Url = url;
            Provider = provider;
        }

        public ReuploadStatus Status { get; }
        public string Url { get; set; }
        public string Provider { get; set; }

        public ReuplaodPayload(ReuploadStatus status)
        {
            Status = status;
        }

        public enum ReuploadStatus
        {
            DownloadFailed,
            UploadFailed,
            Ok,
        }
    }
}