using Notifier.ui;

namespace Notifier.domain.controller
{
    class TaskController
    {
        private model.Task? selectedTask;
        private model.Task creatingTask;

        public TaskController()
        {
            selectedTask = new();
            creatingTask = new();
        }

        public void ClearCreatingTask() => creatingTask = new();
        public model.Task GetCreatingTask() => creatingTask;
        public model.Task? GetSelectedTask() => selectedTask;
        public void AddDateToCreatingTask(DateInfo selectedDate) => creatingTask.TargetDateList.Add(selectedDate);
        public void RemoveDateFromCreatingTask(DateInfo dateInfo) => creatingTask.TargetDateList?.Remove(dateInfo);
        public void SetSelectedTask(model.Task selectedTask) => this.selectedTask = selectedTask;
    }
}
