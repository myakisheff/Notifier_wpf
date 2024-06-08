using Notifier.ui;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskList taskListUI = new();
        private domain.Task selectedTask = new();

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

            MouseDown += MainWindow_MouseDown;
            CloseBtn.Click += CloseBtn_Click;

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
        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Name == "TaskTitle")
                cntTaskTitle.Content = ((TextBox)sender).Text.Length.ToString() + "/" + ((TextBox)sender).MaxLength.ToString();
            else if (((TextBox)sender).Name == "TaskText")
                cntTaskDesc.Content = ((TextBox)sender).Text.Length.ToString() + "/" + ((TextBox)sender).MaxLength.ToString();

            UpdatePreviewData();
        }

        public void UpdatePreviewData()
        {
            var previewData = new { TaskTitle = TaskTitle.Text, TaskCreationDate = DateTime.Now.ToShortDateString(), TaskDescription = TaskText.Text, TaskTargetDate = DateTime.Now.AddDays(1).ToShortDateString() };

            TaskPreview.DataContext = previewData;
        }
        private void TaskAddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!taskListUI.AddNewTask(TaskTitle.Text, TaskText.Text))
            {
                InfoTask.Content = "Bad params!";
            }
            else
            {
                InfoTask.Content = "Successful!";
                UpdateTaskList();
            }
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
    }
}