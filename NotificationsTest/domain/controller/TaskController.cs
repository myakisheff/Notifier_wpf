
using Notifier.ui;

namespace Notifier.domain.controller
{
    class TaskController
    {
        private Task? selectedTask;
        private Task creatingTask;

        public TaskController()
        {
            selectedTask = new();
            creatingTask = new();
        }

        public void ClearCreatingTask() => creatingTask = new();
        public Task GetCreatingTask() => creatingTask;
        public Task? GetSelectedTask() => selectedTask;
        public void AddDateToCreatingTask(DateInfo selectedDate) => creatingTask.TargetDateList.Add(selectedDate);
        public void RemoveDateFromCreatingTask(DateInfo dateInfo) => creatingTask.TargetDateList?.Remove(dateInfo);
        public void SetSelectedTask(Task selectedTask) => this.selectedTask = selectedTask;
    }
}
