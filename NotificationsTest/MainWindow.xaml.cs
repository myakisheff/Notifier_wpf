using Notifier.domain;
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
        private TimeManager timeManager = new();

        private domain.Task selectedTask = new();
        private TaskStorage taskStorage = new();
        private domain.Task? creatingTask;

        public MainWindow()
        {
            InitializeComponent();

            tbIcon.TrayLeftMouseDown += TbIcon_TrayLeftMouseDown;
            tbIcon.Icon = new Icon(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName + "\\NotifyAppIcon.ico");
            PopupTrayCloseBtn.Click += PopupTrayCloseBtn_Click;

            BtnNotify.Click += NotifyBtnFirst_Click;
            TaskAddBtn.Click += TaskAddBtn_Click;
            ListTasks.SelectionChanged += TasksList_SelectionChanged;
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

            // temporary data
            var data = new[] { 
            new { TaskTitle = "Shop visit", TaskCreationDate ="11.11.2011", TaskDescription="Go to the shop", TaskTargetDate="15.11.2011" },
            new { TaskTitle = "Wosh", TaskCreationDate ="13.11.2011", TaskDescription="", TaskTargetDate="25.01.2012" },
            new { TaskTitle = "Read a book", TaskCreationDate ="14.12.2011", TaskDescription="Read Sister's book", TaskTargetDate="15.11.2015" },
            new { TaskTitle = "Сделать тесты в лмс", TaskCreationDate ="02.06.2024", TaskDescription="Сделать тесты по истории и по Защите информации", TaskTargetDate="05.06.2024" },
            new { TaskTitle = "Сделать тесты в лмс", TaskCreationDate ="02.06.2024", TaskDescription="Сделать тесты по истории и по Защите информации Сделать тесты по истории и по Защите информации Сделать тесты по истории и по Защите информации Сделать тесты по истории и по Защите информации", TaskTargetDate="05.06.2024" }};

            ListTasks.ItemsSource = data;

            UpdatePreviewData();
        }

        private void TaskTargetDateDelBtn_Click(object sender, RoutedEventArgs e)
        {
            
            creatingTask?.TargetDateList?.Remove((DateInfo)TaskTargetDatesList.SelectedItem);
            TaskTargetDatesList.Items.Refresh();
        }

        private void TaskTargetDatesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskTargetDateDelBtn.Visibility = Visibility.Visible;
        }

        private void InitializeNewTask()
        {
            creatingTask = new();
            TaskTargetDatesList.ItemsSource = creatingTask.TargetDateList;
            TaskTargetDatesList.Items.Refresh();
        }

        // Menu section
        private void NotifyBtnFirst_Click(object sender, RoutedEventArgs e)
        {
            NotifyTask.ButtonsNotify(selectedTask);
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        // Task add section
        private void MinutesUpBtn_Click(object sender, RoutedEventArgs e)
        {
            timeManager.AddMinutes(1);
            TaskTargetTimeMinutes.Text = timeManager.GetMinutes();
        }

        private void MinutesDownBtn_Click(object sender, RoutedEventArgs e)
        {
            timeManager.RemoveMinutes(1);
            TaskTargetTimeMinutes.Text = timeManager.GetMinutes();
        }

        private void HoursUpBtn_Click(object sender, RoutedEventArgs e)
        {
            timeManager.AddHours(1);
            TaskTargetTimeHours.Text = timeManager.GetHours();
        }

        private void HoursDownBtn_Click(object sender, RoutedEventArgs e)
        {
            timeManager.RemoveHours(1);
            TaskTargetTimeHours.Text = timeManager.GetHours();
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

        private void TaskTargetTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(sender is TextBox box)
            {
                if (!timeManager.SetNewTime(TaskTargetTimeHours.Text, TaskTargetTimeMinutes.Text))
                    box.Foreground = new SolidColorBrush(Colors.MediumVioletRed);
                else
                    box.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TaskTargetDateAddBtn_Click(object sender, RoutedEventArgs e)
        {        
            creatingTask?.TargetDateList?.Add(new DateInfo()
            {
                ID = creatingTask?.TargetDateList?.Count == 0 ? 0 : creatingTask?.TargetDateList?.Last().ID + 1,
                Date = TaskTargetDate?.SelectedDate!.Value.ToShortDateString(),
                Time = timeManager.CurrentTime,
            });

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
            var previewData = new { TaskTitle = TaskTitle.Text, TaskCreationDate = DateTime.Now.ToShortDateString(), TaskDescription = TaskText.Text, TaskTargetDate = DateTime.Now.AddDays(1).ToString() };

            TaskPreview.DataContext = previewData;
        }
        private void TaskAddBtn_Click(object sender, RoutedEventArgs e)
        {
            InfoTask.Visibility = Visibility.Visible;
            var bc = new BrushConverter();
            //if (!taskListUI.AddNewTask(TaskTitle.Text, TaskText.Text))
            //{
            //    InfoTask.Content = (string)Application.Current.FindResource("m_TaskCreationWrong");
            //    InfoTask.Foreground = (System.Windows.Media.Brush?)bc.ConvertFrom("#b91316");
            //}
            //else
            //{
            //    InfoTask.Content = (string)Application.Current.FindResource("m_TaskCreationSuccess");
            //    InfoTask.Foreground = (System.Windows.Media.Brush?)bc.ConvertFrom("#10790e");
            //    UpdateTaskList();
            //}
        }

        // Task List section

        private void MenuItemTile_Click(object sender, RoutedEventArgs e)
        {
            ListTasks.layout = Layout.Tile;
        }

        private void MenuItemList_Click(object sender, RoutedEventArgs e)
        {
            ListTasks.layout = Layout.List;
        }

        private void UpdateTaskList()
        {
            // TasksList.ItemsSource = taskListUI.updateTaskList();
        }
        private void TasksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBox)sender;
            selectedTask = (domain.Task)item.SelectedItem;
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