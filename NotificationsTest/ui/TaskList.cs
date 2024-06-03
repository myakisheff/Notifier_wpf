using Notifier.domain;

namespace Notifier.ui
{
    internal class TaskList
    {
        private TaskStorage taskStorage = new();

        public IList<domain.Task> updateTaskList()
        {
            IList<domain.Task> co1 = new List<domain.Task>();

            foreach (domain.Task dr in taskStorage.GetAllTasks())
            {
                co1.Add(new domain.Task
                {
                    id = dr.id,
                    label = dr.label,
                    text = dr.text
                });
            }
            
            return co1;
        }

        public bool AddNewTask(string label, string text)
        {
            if (label == "" || label == null || text == "" || text == null)
                return false;

            taskStorage.AddTask(new domain.Task(label, text));
            return true;
        }
    }
}
