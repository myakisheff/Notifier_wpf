namespace Notifier.domain.repository
{
    interface ITaskRepository
    {
        IEnumerable<model.Task> GetTaskList();
        void Create(model.Task task);
        model.Task? GetById(int id);
        void Update(model.Task task);
        void DeleteById(int id);
        void Save();
    }
}
