namespace Notifier.domain.repository
{
    class FakeTaskRepositoryImpl : ITaskRepository
    {
        private List<model.Task> TaskList;

        public FakeTaskRepositoryImpl()
        {
            TaskList = new();
        }

        public List<model.Task> GetTaskList()
        {
            return TaskList;
        }

        public model.Task? GetById(int id)
        {
            return TaskList.Find(task => task.ID == id);
        }

        public void Create(model.Task task)
        {
            TaskList.Add(task);
        }

        public void DeleteById(int id)
        {
            model.Task? task = TaskList.Find(task => task.ID == id);
            if(task != null)
            {
                TaskList.Remove(task);
            }
        }

        public void Update(model.Task task)
        {
            int taskIndex = TaskList.FindIndex(task => task.ID == task.ID);
            if (taskIndex != -1)
            {
                TaskList[taskIndex] = task;
            }
        }

        public void Save()
        {
            // Nothing to do, couse it's a fake impl
        }

        public void Refresh()
        {
            // Nothing
        }
    }
}
