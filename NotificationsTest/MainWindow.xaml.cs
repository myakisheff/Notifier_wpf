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

            NotifyBtnFirst.Click += NotifyBtnFirst_Click;
            TaskAddBtn.Click += TaskAddBtn_Click;
            TasksList.SelectionChanged += TasksList_SelectionChanged;

            MouseDown += MainWindow_MouseDown;
            CloseBtn.Click += CloseBtn_Click;
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
            TasksList.ItemsSource = taskListUI.updateTaskList();
        }

        public void LoadData()
        {

        }
    }
}