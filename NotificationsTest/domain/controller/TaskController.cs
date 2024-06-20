using Notifier.domain.model;
using Notifier.domain.repository;

namespace Notifier.domain.controller
{
    class TaskController
    {
        private model.Task? selectedTask;
        private model.Task creatingTask;

        private readonly ITaskRepository taskRepository = new SSSTaskRepositoryImpl();

        public TaskController()
        {
            selectedTask = new();
            creatingTask = new();
        }

        public void ClearCreatingTask() => creatingTask = new();
        public model.Task GetCreatingTask() => creatingTask;
        public model.Task? GetSelectedTask() => selectedTask;
        public void RemoveDateFromCreatingTask(DateInfo dateInfo) => creatingTask.TargetDateList?.Remove(dateInfo);
        public void SetSelectedTask(model.Task selectedTask) => this.selectedTask = selectedTask;
        public List<model.Task> GetTaskList() => taskRepository.GetTaskList();
        public void RefreshTasks() => taskRepository.Refresh();
        public void SaveTasks() => taskRepository.Save();

        public bool CreateTask()
        {
            creatingTask.TaskCreationDate = DateTime.Now.ToShortDateString();
            creatingTask.ID = GetTaskList().Any() ? GetTaskList().Last().ID + 1 : 0;

            if (String.IsNullOrEmpty(creatingTask.TaskTitle) || creatingTask.TargetDateList == null || creatingTask.TargetDateList.Count == 0)
                return false;

            taskRepository.Create(creatingTask);
            return true;
        }

        internal void UpdateTaskList()
        {
            List<int> taskIdsToDelete = new();

            foreach(var task in GetTaskList())
            {
                task.TaskTargetDate = task.GetNearestDate().ToString();
                if (task.GetNearestDate() == null)
                {
                    taskIdsToDelete.Add(task.ID);
                }
            }

            foreach(int id in taskIdsToDelete)
            {
                taskRepository.DeleteById(id);
            }
        }

        public DateErrorType AddDateToCreatingTask(string date, string time)
        {
            DateInfo selectedDate = new()
            {
                ID = creatingTask.TargetDateList.Any() ? creatingTask.TargetDateList.Last().ID + 1 : 0,
                Date = date,
                Time = time,
            };

            foreach (DateInfo tDate in creatingTask.TargetDateList)
            {
                if (tDate.Date == selectedDate.Date && tDate.Time == selectedDate.Time)
                {
                    return DateErrorType.DateExists;
                }
            }

            DateTime choosedDate = DateTime.ParseExact($"{selectedDate.Date} {selectedDate.Time}", "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

            if (choosedDate < DateTime.Now)
            {
                return DateErrorType.WrongDate;
            }

            creatingTask.TargetDateList.Add(selectedDate);

            return DateErrorType.None;
        }
    }
}
