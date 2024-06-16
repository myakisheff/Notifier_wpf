using Notifier.ui;

namespace Notifier.domain.controller
{
    internal class NotifyController
    {
        private List<(model.Task task, DateTime date)> upcomingTasksPool;

        private System.Timers.Timer timerUpcomings;
        private System.Timers.Timer timerTimeChecker;

        private List<model.Task>? tasks;

        internal List<model.Task>? Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                CheckDates();
            }
        }

        public NotifyController() 
        {
            upcomingTasksPool = new();

            timerUpcomings = new();
            timerUpcomings.Interval = 1800000; // 30 minutes
            timerUpcomings.Elapsed += TimerUpcomings_Elapsed;

            timerTimeChecker = new();
            timerTimeChecker.Interval = 1000;
            timerTimeChecker.Elapsed += TimerTimeChecker_Elapsed;

            timerUpcomings.Start();
            timerTimeChecker.Start();
        }

        private void TimerTimeChecker_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currentDate = DateTime.Now;

            List<(model.Task task, DateTime date)> tasksToRemove = new();

            foreach(var task in upcomingTasksPool)
            {
                if(task.date <= currentDate)
                {
                    NotifyTask.Notify(task.task);
                    tasksToRemove.Add(task);
                }
            }

            foreach (var task in tasksToRemove)
            {
                upcomingTasksPool.Remove(task);
            }

        }

        private void TimerUpcomings_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            CheckDates();
        }

        public void CheckDates()
        {
            if (tasks == null)
                return;

            DateTime currentDate = DateTime.Now;

            upcomingTasksPool.Clear();

            foreach (model.Task task in tasks)
            {
                var near = task.GetNearestDate();

                if(near == null)
                    continue;

                if (currentDate - near < new TimeSpan(50, 0, 0))
                {
                    foreach(var date in task.TargetDateList)
                    {
                        DateTime dateFromList = DateTime.ParseExact($"{date.Date} {date.Time}", "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

                        upcomingTasksPool.Add((task, dateFromList));
                    }
                }
            }
        }
    }
}
