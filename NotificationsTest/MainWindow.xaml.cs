using Notifier.domain.controller;
using Notifier.domain.model;
using Notifier.domain.repository;
using Notifier.ui;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Notifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ITaskRepository taskRepository = new FakeTaskRepositoryImpl();
        private readonly TaskController taskController = new();
        private readonly TimeController timeController = new();
        private readonly NotifyController notifyController = new();

        public MainWindow()
        {
            InitializeComponent();

            tbIcon.TrayLeftMouseDown += TbIcon_TrayLeftMouseDown;
            tbIcon.Icon = new Icon(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName + "\\NotifyAppIcon.ico");
            PopupTrayCloseBtn.Click += PopupTrayCloseBtn_Click;

            BtnRefresh.Click += BtnRefresh_Click;
            BtnNotify.Click += NotifyBtnFirst_Click;
            TaskAddBtn.Click += TaskAddBtn_Click;
            ListTasks.SelectionChanged += ListTasks_SelectionChanged;
            TaskTitle.TextChanged += Input_TextChanged;
            TaskText.TextChanged += Input_TextChanged;
            SwitchTaskBtn.Click += SwitchTaskBtn_Click;
            TaskTargetDateAddBtn.Click += TaskTargetDateAddBtn_Click;

            TaskTargetTimeHours.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPasteCommand));
            TaskTargetTimeMinutes.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPasteCommand));

            TaskTargetTimeHours.LostFocus += TaskTargetTime_LostFocus;
            TaskTargetTimeMinutes.LostFocus += TaskTargetTime_LostFocus;

            TaskTargetTimeHours.TextChanged += TaskTargetTime_TextChanged;
            TaskTargetTimeMinutes.TextChanged += TaskTargetTime_TextChanged;

            MouseDown += MainWindow_MouseDown;
            CloseBtn.Click += CloseBtn_Click;

            HoursDownBtn.Click += HoursDownBtn_Click;
            HoursUpBtn.Click += HoursUpBtn_Click;
            MinutesDownBtn.Click += MinutesDownBtn_Click;
            MinutesUpBtn.Click += MinutesUpBtn_Click;

            TaskTargetDatesList.SelectionChanged += TaskTargetDatesList_SelectionChanged;
            TaskTargetDateDelBtn.Click += TaskTargetDateDelBtn_Click;

            InitializeNewTask();

            TaskTargetDate.SelectedDate = DateTime.Now;
            notifyController.Tasks = (List<domain.model.Task>)taskRepository.GetTaskList();

            ListTasks.ItemsSource = taskRepository.GetTaskList();

            UpdatePreviewData();
        }

        private void InitializeNewTask()
        {
            taskController.ClearCreatingTask();
            TaskTargetDatesList.ItemsSource = taskController.GetCreatingTask().TargetDateList;
            TaskTitle.Text = "";
            TaskText.Text = "";
            TaskTargetDatesList.Items.Refresh();
        }

        // Menu section
        private void NotifyBtnFirst_Click(object sender, RoutedEventArgs e)
        {
            NotifyTask.Notify(taskController.GetSelectedTask());
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateListTasksData();
        }

        // Task add section
        private void MinutesUpBtn_Click(object sender, RoutedEventArgs e)
        {
            timeController.AddMinutes(1);
            TaskTargetTimeMinutes.Text = timeController.GetMinutesString();
        }

        private void MinutesDownBtn_Click(object sender, RoutedEventArgs e)
        {
            timeController.RemoveMinutes(1);
            TaskTargetTimeMinutes.Text = timeController.GetMinutesString();
        }

        private void HoursUpBtn_Click(object sender, RoutedEventArgs e)
        {
            timeController.AddHours(1);
            TaskTargetTimeHours.Text = timeController.GetHoursString();
        }

        private void HoursDownBtn_Click(object sender, RoutedEventArgs e)
        {
            timeController.RemoveHours(1);
            TaskTargetTimeHours.Text = timeController.GetHoursString();
        }

        private void SwitchTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if(TaskTitleDescPanel.Visibility == Visibility.Visible)
            {
                TaskTitleDescPanel.Visibility = Visibility.Collapsed;
                TaskTargetDatesPanel.Visibility = Visibility.Visible;
            }
            else
            {
                TaskTitleDescPanel.Visibility = Visibility.Visible;
                TaskTargetDatesPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void TaskTargetDateDelBtn_Click(object sender, RoutedEventArgs e)
        {
            taskController.RemoveDateFromCreatingTask((DateInfo)TaskTargetDatesList.SelectedItem);
            TaskTargetDatesList.Items.Refresh();
        }

        private void TaskTargetDatesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TaskTargetDatesList.SelectedItem != null)
                TaskTargetDateDelBtn.Visibility = Visibility.Visible;
            else TaskTargetDateDelBtn.Visibility = Visibility.Collapsed;
        }

        private void TaskTargetTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(sender is TextBox box)
            {
                if(box.Name == "TaskTargetTimeHours") 
                { 
                    if(!timeController.SetNewHour(box.Text))
                        box.Foreground = new SolidColorBrush(Colors.MediumVioletRed);
                    else
                        box.Foreground = new SolidColorBrush(Colors.Black);
                }
                else if(box.Name == "TaskTargetTimeMinutes")
                {
                    if (!timeController.SetNewMinute(box.Text))
                        box.Foreground = new SolidColorBrush(Colors.MediumVioletRed);
                    else
                        box.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void TaskTargetDateAddBtn_Click(object sender, RoutedEventArgs e)
        {
            DateInfo selectedDate = new()
            {
                ID = taskController.GetCreatingTask().TargetDateList.Any() ? taskController.GetCreatingTask().TargetDateList.Last().ID + 1 : 0,
                Date = TaskTargetDate?.SelectedDate!.Value.ToShortDateString(),
                Time = timeController.GetTimeString(),
            };

            foreach(DateInfo tDate in taskController.GetCreatingTask().TargetDateList)
            {
                if (tDate.Date == selectedDate.Date && tDate.Time == selectedDate.Time)
                {
                    MessageBox.Show((string)Application.Current.FindResource("m_RepeatedDate"));
                    return;
                }
            }

            DateTime choosedDate = DateTime.ParseExact($"{selectedDate.Date} {selectedDate.Time}", "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

            if (choosedDate < DateTime.Now)
            {
                MessageBox.Show((string)Application.Current.FindResource("m_WrongDate"));
                return;
            }

            taskController.AddDateToCreatingTask(selectedDate);

            UpdatePreviewData();
            TaskTargetDatesList.Items.Refresh();
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (string.IsNullOrEmpty(box.Text))
                    box.Background = (ImageBrush)FindResource("watermarkTime");
                else
                    box.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            InfoTask.Visibility = Visibility.Hidden;

            int txtLength = ((TextBox)sender).Text.Length;
            int txtMaxLength = ((TextBox)sender).MaxLength;
            string labelText = "";

            if (txtLength != 0)
                labelText = txtLength.ToString() + "/" + txtMaxLength.ToString();

            if (((TextBox)sender).Name == "TaskTitle")
                cntTaskTitle.Content = labelText;
            else if (((TextBox)sender).Name == "TaskText")
                cntTaskDesc.Content = labelText;

            UpdatePreviewData();
        }

        public void UpdatePreviewData()
        {
            var previewData = new { TaskTitle = TaskTitle.Text, TaskCreationDate = DateTime.Now.ToShortDateString(), TaskDescription = TaskText.Text, TaskTargetDate = taskController.GetCreatingTask()?.GetNearestDate()?.ToString() };

            TaskPreview.DataContext = previewData;
        }
        private void TaskAddBtn_Click(object sender, RoutedEventArgs e)
        {
            InfoTask.Visibility = Visibility.Visible;

            taskController.GetCreatingTask().TaskCreationDate = DateTime.Now.ToShortDateString();
            taskController.GetCreatingTask().TaskTitle = TaskTitle.Text;
            taskController.GetCreatingTask().TaskDescription = TaskText.Text;

            domain.model.Task creatingTask = taskController.GetCreatingTask();

            if (String.IsNullOrEmpty(creatingTask.TaskTitle) || creatingTask.TargetDateList == null || creatingTask.TargetDateList.Count == 0)
            { 
                WrongTaskData();
                return;
            }

            creatingTask.ID = taskRepository.GetTaskList().Any() ? taskRepository.GetTaskList().Last().ID + 1 : 0;

            taskRepository.Create(creatingTask);

            InfoTask.Content = (string)Application.Current.FindResource("m_TaskCreationSuccess");
            InfoTask.Foreground = (System.Windows.Media.Brush?)new BrushConverter().ConvertFrom("#10790e");

            InitializeNewTask();
            UpdatePreviewData();
            UpdateListTasksData();

            notifyController.CheckDates();
        }

        private void UpdateListTasksData()
        {
            List<int> taskIdsToDelete = new(); 

            foreach(var task in taskRepository.GetTaskList())
            {
                task.TaskTargetDate = task.GetNearestDate().ToString();
                if(task.GetNearestDate() == null)
                {
                    taskIdsToDelete.Add(task.ID);
                }
            }

            foreach (int id in taskIdsToDelete)
            {
                taskRepository.DeleteById(id);
            }

            ListTasks.Items.Refresh();
        }

        private void WrongTaskData()
        {
            var bc = new BrushConverter();
            InfoTask.Content = (string)Application.Current.FindResource("m_TaskCreationWrong");
            InfoTask.Foreground = (System.Windows.Media.Brush?)bc.ConvertFrom("#b91316");
        }

        // Task List section
        private void ListTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            taskController.SetSelectedTask((domain.model.Task)((LayoutListTasks)sender).SelectedItem);
        }

        private void MenuItemTile_Click(object sender, RoutedEventArgs e)
        {
            ListTasks.layout = Layout.Tile;
        }

        private void MenuItemList_Click(object sender, RoutedEventArgs e)
        {
            ListTasks.layout = Layout.List;
        }

        // not visable section
        private void PopupTrayCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            tbIcon.TrayPopupResolved.IsOpen = false;
            Close();
        }

        private void TbIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            Show();
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void TaskTargetTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void TaskTargetTime_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (box.Text.Length == 1)
                    box.Text = "0" + box.Text;
            }
        }

        public void OnPasteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            // Do nothing (for restricting paste)
        }
    }
}