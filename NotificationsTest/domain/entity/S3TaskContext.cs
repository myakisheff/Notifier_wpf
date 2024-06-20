using Amazon.S3;
using Amazon.S3.Util;
using Notifier.domain.model;
using Notifier.utils;
using System.Windows;

namespace Notifier.domain.entity
{
    internal class S3TaskContext
    {
        public List<model.Task> Tasks { get; set; }

        private string BucketName = "task-storage";
        private AmazonS3Client s3client;

        public bool IsBucketFound { get; private set; }

        public S3TaskContext()
        {
            Tasks = new List<model.Task>();

            AmazonS3Config configsS3 = new()
            {
                ServiceURL = "https://s3.yandexcloud.net",
                AllowAutoRedirect = true,
            };
            s3client = new(configsS3);

        }

        public void SaveTasks()
        {
            
        }

        //public async void SendTasks()
        //{
        //    //var response = await s3client.PutObjectAsync();
        //}

        //public async Task GetTasks()
        //{
            
        //}

        //public async Task CheckBucket()
        //{
        //    var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3client, BucketName);

        //    if (!bucketExists)
        //    {
        //        IsBucketFound = false;
        //        MessageBox.Show($"Bucket {BucketName} not found");
        //    }
        //    else
        //        IsBucketFound = true;
        //}

    }
}
