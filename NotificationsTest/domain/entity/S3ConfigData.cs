namespace Notifier.domain.entity
{
    internal class S3ConfigData
    {
        public string? BucketName { get; set; }
        public string? ServiceURL { get; set; }
        public string? AuthenticationRegion { get; set; }
        public string? AccessKey { get; set; }
        public string? SecretKey { get; set; }
    }
}
