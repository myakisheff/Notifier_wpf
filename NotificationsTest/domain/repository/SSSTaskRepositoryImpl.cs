
using Amazon.S3;

namespace Notifier.domain.repository
{
    internal class SSSTaskRepositoryImpl : ITaskRepository
    {
        private List<model.Task> TaskList;

        private string accessKey = "YCAJEbcpGsxDG9-feFfWpRoeh";
        private string secretKey = "YCP0zh1uVdLNZMN0Iwd3iBaEflGfOQzdmIao7cXv";

        public SSSTaskRepositoryImpl()
        {
            TaskList = new();

            AmazonS3Config configsS3 = new()
            {
                ServiceURL = "https://s3.yandexcloud.net",
            };

            AmazonS3Client s3client = new(configsS3);

            

        }

        public void Create(model.Task task)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public model.Task? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<model.Task> GetTaskList()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(model.Task task)
        {
            throw new NotImplementedException();
        }
    }
}
