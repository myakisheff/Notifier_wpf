using Notifier.ui;
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
            tbIcon.Icon = new System.Drawing.Icon(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName + "\\NotifyAppIcon.ico");
            PopupTrayCloseBtn.Click += PopupTrayCloseBtn_Click;

            BtnNotify.Click += NotifyBtnFirst_Click;
            TaskAddBtn.Click += TaskAddBtn_Click;
            ListTasks.SelectionChanged += TasksList_SelectionChanged;

            MouseDown += MainWindow_MouseDown;
            CloseBtn.Click += CloseBtn_Click;

            var data = new[] { 
            new { TaskTitle = "Shop visit", TaskCreationDate ="11.11.2011", TaskDescription="Go to the shop", TaskTargetDate="15.11.2011" },
            new { TaskTitle = "Wosh", TaskCreationDate ="13.11.2011", TaskDescription="", TaskTargetDate="25.01.2012" },
            new { TaskTitle = "Read a book", TaskCreationDate ="14.12.2011", TaskDescription="Read Sister's book", TaskTargetDate="15.11.2015" },
            new { TaskTitle = "Сделать тесты в лмс", TaskCreationDate ="02.06.2024", TaskDescription="Сделать тесты по истории и по Защите информации", TaskTargetDate="05.06.2024" },
            new { TaskTitle = "Сделать тесты в лмс", TaskCreationDate ="02.06.2024", TaskDescription="Сделать тесты по истории и по Защите информации Сделать тесты по истории и по Защите информации Сделать тесты по истории и по Защите информации Сделать тесты по истории и по Защите информации", TaskTargetDate="05.06.2024" }};

            ListTasks.ItemsSource = data;
        }

        private void PopupTrayCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TbIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            Show();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void TasksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBox)sender;
            selectedTask = (domain.Task)item.SelectedItem;
        }

        private void TaskAddBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!taskListUI.AddNewTask(TaskLabel.Text, TaskText.Text))
            {
                InfoTask.Content = "Bad params!";
            }
            else
            {
                InfoTask.Content = "Successful!";
                UpdateTaskList();
            }
        }

        private void NotifyBtnFirst_Click(object sender, RoutedEventArgs e)
        {
            NotifyTask.ButtonsNotify(selectedTask);
        }

        private void UpdateTaskList()
        {
            // TasksList.ItemsSource = taskListUI.updateTaskList();
        }

        public void LoadData()
        {

        }

        private void MenuItemTile_Click(object sender, RoutedEventArgs e)
        {
            ListTasks.layout = Layout.Tile;
        }

        private void MenuItemList_Click(object sender, RoutedEventArgs e)
        {
            ListTasks.layout = Layout.List;
        }
    }
}