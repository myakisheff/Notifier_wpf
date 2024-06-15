namespace Notifier.domain.repository
{
    interface ITaskRepository
    {
        IEnumerable<Task> GetTaskList();
        void Create(Task task);
        Task? GetById(int id);
        void Update(Task task);
        void DeleteById(int id);
        void Save();
    }
}
