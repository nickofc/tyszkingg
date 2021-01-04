namespace TwReplay.Reupload
{
    public class ReuploadPayload
    {
        public ReuploadPayload(ReuploadStatus status,
            string url = null, string provider = null)
        {
            Status = status;
            Url = url;
            Provider = provider;
        }

        public ReuploadStatus Status { get; }
        public string Url { get; set; }
        public string Provider { get; set; }
    }

    public enum ReuploadStatus
    {
        DownloadFailed,
        UploadFailed,
        Ok,
    }
}