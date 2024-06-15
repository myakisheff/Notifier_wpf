
namespace Notifier.domain.repository
{
    class FakeTaskRepositoryImpl : ITaskRepository
    {
        private List<Task> TaskList;

        public FakeTaskRepositoryImpl()
        {
            TaskList = new();
        }

        public IEnumerable<Task> GetTaskList()
        {
            return TaskList;
        }

        public Task? GetById(int id)
        {
            return TaskList.Find(task => task.id == id);
        }

        public void Create(Task task)
        {
            TaskList.Add(task);
        }

        public void DeleteById(int id)
        {
            Task? task = TaskList.Find(task => task.id == id);
            if(task != null)
            {
                TaskList.Remove(task);
            }
        }

        public void Update(Task task)
        {
            int taskIndex = TaskList.FindIndex(task => task.id == task.id);
            if (taskIndex != -1)
            {
                TaskList[taskIndex] = task;
            }
        }

        public void Save()
        {
            // Nothing to do, couse it's a fake impl
        }
    }
}
