namespace Notifier.domain
{
    internal class TaskStorage
    {
        public List<Task> Tasks { get; private set; }

        public TaskStorage() 
        {
            Tasks = new();
        }

        public TaskStorage(List<Task> tasks)
        {
            Tasks = tasks;
        }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            Tasks.Remove(task);
        }

        public void Clear()
        {
            Tasks.Clear();
        }

        public bool Contains(Task task)
        {
            return Tasks.Contains(task);
        }

        public List<Task> GetAllTasks()
        {
            return Tasks;
        }

        public Task GetLastTask()
        {
            return Tasks.Last();
        }
    }
}
