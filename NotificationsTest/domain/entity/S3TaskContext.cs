using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Notifier.utils;
using System.IO;
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

            _ = CheckBucket();
        }

        public async void SaveTasks()
        {
            TaskJSONManager.Write(Tasks);
            await SendTasks();
        }

        public async Task SendTasks()
        {
            using (FileStream fstream = new FileStream(TaskJSONManager.GetPath(), FileMode.OpenOrCreate))
            {
                var objectRequest = new PutObjectRequest()
                {
                    BucketName = BucketName,
                    Key = "Tasks.json",
                    InputStream = fstream,
                };

                await s3client.PutObjectAsync(objectRequest);
            }
        }

        public async void GetTasks()
        {
            GetObjectResponse res;
            try
            {
                res = await s3client.GetObjectAsync(BucketName, "Tasks.json");
            }
            catch (Exception)
            {
                return;
            }

            var fileStream = File.Create(TaskJSONManager.GetPath());
            res.ResponseStream.CopyTo(fileStream);
            fileStream.Close();
            res.ResponseStream.Close();

            var loadedTasks = TaskJSONManager.Read();

            Tasks = loadedTasks is null ? Tasks : loadedTasks;
        }

        public async Task CheckBucket()
        {
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3client, BucketName);

            if (!bucketExists)
            {
                IsBucketFound = false;
                MessageBox.Show($"Bucket {BucketName} not found");
            }
            else
                IsBucketFound = true;
        }

    }
}
