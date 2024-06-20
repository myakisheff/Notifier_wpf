using Notifier.domain.entity;

namespace Notifier.domain.repository
{
    internal class SSSTaskRepositoryImpl : ITaskRepository
    {
        private S3TaskContext storage;

        private string accessKey = "YCAJEbcpGsxDG9-feFfWpRoeh";
        private string secretKey = "YCP0zh1uVdLNZMN0Iwd3iBaEflGfOQzdmIao7cXv";

        public SSSTaskRepositoryImpl()
        {
            storage = new();
        }

        public void Create(model.Task task)
        {
            storage.Tasks.Add(task);
        }

        public void DeleteById(int id)
        {
            
        }

        public model.Task? GetById(int id)
        {
            return null;
        }

        public IEnumerable<model.Task> GetTaskList()
        {
            return storage.Tasks;
        }

        public void Save()
        {
            storage.SaveTasks();
        }

        public void Update(model.Task task)
        {
            
        }
    }
}
