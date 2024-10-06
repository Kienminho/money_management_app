namespace Common.Settings
{
    public class AppSettings
    {
        public string SmsToken { get; set; }
        public string SmsServiceUrl { get; set; }
        public string StrStorageFileAzure { get; set; }
        public string StrContainerRefer { get; set; }
        public string FolderUploadFile { get; set; }
    }

    public class StrJWT
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }

    public class ConnectionStrings
    {
        public string WebApiDatabase { get; set; }
    }

    public class ApplicationInsights
    {
        public string ConnectionString { get; set; }
    }
}