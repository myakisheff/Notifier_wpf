using Notifier.domain.controller;
using Notifier.domain.model;
using Notifier.ui;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Notifier
{
    public partial class MainWindow : Window
    {
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
            notifyController.Tasks = taskController.GetTaskList();

            ListTasks.ItemsSource = taskController.GetTaskList();

            _ = RunPeriodicSave();

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
            taskController.RefreshTasks();
            notifyController.CheckDates();
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
            if (TaskTargetDate.SelectedDate is null)
            {
                MessageBox.Show((string)Application.Current.FindResource("m_NeedToChooseDate"));
                return;
            }

            switch (taskController.AddDateToCreatingTask(TaskTargetDate.SelectedDate.Value.ToShortDateString(), timeController.GetTimeString()))
            {
                case DateErrorType.None:
                    break;
                case DateErrorType.DateExists:
                    MessageBox.Show((string)Application.Current.FindResource("m_RepeatedDate"));
                    break;
                case DateErrorType.WrongDate:
                    MessageBox.Show((string)Application.Current.FindResource("m_WrongDate"));
                    break;
                    
            }

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
            if (sender is not TextBox)
                return;

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
            TaskPreview.DataContext = new { 
                TaskTitle = TaskTitle.Text, 
                TaskCreationDate = DateTime.Now.ToShortDateString(), 
                TaskDescription = TaskText.Text, 
                TaskTargetDate = taskController.GetCreatingTask()?.GetNearestDate()?.ToString(),
            };
        }
        private void TaskAddBtn_Click(object sender, RoutedEventArgs e)
        {
            InfoTask.Visibility = Visibility.Visible;

            taskController.GetCreatingTask().TaskTitle = TaskTitle.Text;
            taskController.GetCreatingTask().TaskDescription = TaskText.Text;

            if (!taskController.CreateTask())
            { 
                WrongTaskData();
                return;
            }

            taskController.SaveTasks();

            InfoTask.Content = (string)Application.Current.FindResource("m_TaskCreationSuccess");
            InfoTask.Foreground = (System.Windows.Media.Brush?)new BrushConverter().ConvertFrom("#10790e");

            InitializeNewTask();
            UpdatePreviewData();
            UpdateListTasksData();

            notifyController.CheckDates();
        }

        async System.Threading.Tasks.Task RunPeriodicSave()
        {
            while (true)
            {
                await System.Threading.Tasks.Task.Delay(2500);
                taskController.UpdateTaskList();
                ListTasks.ItemsSource = taskController.GetTaskList();
                ListTasks.Items.Refresh();
            }
        }

        private void UpdateListTasksData()
        {
            taskController.UpdateTaskList();
            ListTasks.ItemsSource = taskController.GetTaskList();
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