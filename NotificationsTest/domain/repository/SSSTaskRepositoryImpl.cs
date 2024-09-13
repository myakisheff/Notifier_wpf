using Notifier.domain.entity;

namespace Notifier.domain.repository
{
    internal class SSSTaskRepositoryImpl : ITaskRepository
    {
        private readonly S3TaskContext storage;

        public SSSTaskRepositoryImpl()
        {
            storage = new();
            storage.GetTasks();
        }

        public void Create(model.Task task)
        {
            storage.Tasks.Add(task);
        }

        public void DeleteById(int id)
        {
            model.Task? task = storage.Tasks.Find(task => task.ID == id);
            if (task != null)
            {
                storage.Tasks.Remove(task);
            }
        }

        public model.Task? GetById(int id)
        {
            return storage.Tasks.Find(task => task.ID == id);
        }

        public List<model.Task> GetTaskList()
        {
            return storage.Tasks;
        }

        public void Refresh()
        {
            storage.GetTasks();
        }

        public void Save()
        {
            Thread thread = new(storage.SaveTasks);
            thread.Start();
        }

        public void Update(model.Task task)
        {
            int taskIndex = storage.Tasks.FindIndex(task => task.ID == task.ID);
            if (taskIndex != -1)
            {
                storage.Tasks[taskIndex] = task;
            }
        }
    }
}
