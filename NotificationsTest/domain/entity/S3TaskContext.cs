using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Notifier.utils;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace Notifier.domain.entity
{
    internal class S3TaskContext
    {
        public List<model.Task> Tasks { get; set; }

        private readonly string? BucketName;
        private readonly AmazonS3Client? s3client;

        public bool IsBucketFound { get; private set; }

        public S3TaskContext()
        {
            Tasks = new List<model.Task>();
            var config = GetConfigConnectionFromXML();

            if(config is null)
            {
                MessageBox.Show("Bad connection config");
                return;
            }

            BucketName = config[0].BucketName;
            AmazonS3Config configsS3 = new()
            {
                ServiceURL = config[0].ServiceURL,
                AuthenticationRegion = config[0].AuthenticationRegion,
            };
            s3client = new(config[0].AccessKey, config[0].SecretKey, configsS3);

            _ = CheckBucket();
        }

        public async void SaveTasks()
        {
            TaskJSONManager.Write(Tasks);
            await SendTasks();
        }

        public async Task SendTasks()
        {
            if (s3client is null)
                return;

            using FileStream fstream = new(TaskJSONManager.GetPath(), FileMode.OpenOrCreate);
            var objectRequest = new PutObjectRequest()
            {
                BucketName = BucketName,
                Key = "Tasks.json",
                InputStream = fstream,
            };

            await s3client.PutObjectAsync(objectRequest);
        }

        public async void GetTasks()
        {
            if (s3client is null)
                return;

            GetObjectResponse res;
            try
            {
                res = await s3client.GetObjectAsync(BucketName, "Tasks.json");
            }
            catch (AmazonS3Exception)
            {
                return;
            }

            StreamWriter? writer;

            try
            {
                writer = new StreamWriter(TaskJSONManager.GetPath(), false);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            res.ResponseStream.CopyTo(writer.BaseStream);
            writer.Close();
            res.ResponseStream.Close();

            var loadedTasks = TaskJSONManager.Read();

            Tasks = loadedTasks is null ? Tasks : loadedTasks;
        }

        public async Task CheckBucket()
        {
            bool bucketExists = false;

            try
            {
                bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3client, BucketName);
            }catch(AmazonS3Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            if (!bucketExists)
            {
                IsBucketFound = false;
                MessageBox.Show($"Unable to connect to storage. Bucket {BucketName} not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                IsBucketFound = true;
            }
        }

        public static List<S3ConfigData>? GetConfigConnectionFromXML()
        {
            var sri = Application.GetResourceStream(
                    new Uri("ConnectionConfig.xml", UriKind.Relative));

            var xdoc = XDocument.Load(sri.Stream);

            var config = xdoc?.Root?.Descendants("S3Connection").Select(x => new S3ConfigData()
            {
                BucketName = x.Element("BucketName")?.Value,
                ServiceURL = x.Element("ServiceURL")?.Value,
                AuthenticationRegion = x.Element("AuthenticationRegion")?.Value,
                AccessKey = x.Element("AccessKey")?.Value,
                SecretKey = x.Element("SecretKey")?.Value,
            }).ToList();

            return config;
        }
    }
}
