namespace Notifier.domain
{
    internal class TaskStorage
    {
        private List<Task> tasks;

        public TaskStorage() 
        { 
            tasks = new();
        }

        public TaskStorage(List<Task> tasks)
        {
            this.tasks = tasks;
        }

        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            tasks.Remove(task);
        }

        public void Clear()
        {
            tasks.Clear();
        }

        public bool Contains(Task task)
        {
            return tasks.Contains(task);
        }

        public List<Task> GetAllTasks()
        {
            return tasks;
        }
    }
}
