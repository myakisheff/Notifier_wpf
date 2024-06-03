using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Notifier
{
    public partial class Loader : Window, INotifyPropertyChanged
    {
        private int _workerState;

        private MainWindow window = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public int WorkerState
        {
            get { return _workerState; }
            set 
            {
                
                _workerState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WorkerState)));
            }
        }

        public Loader()
        {
            InitializeComponent();
            DataContext = this;

            UIWindow.MouseDown += Loader_MouseDown;

            string cursorDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName + "\\assets\\cursors";
            UIWindow.Cursor = new Cursor($"{cursorDirectory}\\cursor.cur");

            CloseBtn.Click += Loader_CloseBtn;

            window.LoadData();

            StartProgressBar();
        }

        private void SetTheme()
        {
            var uri = new Uri("resourceDictionaries/themes/DarkTheme.xaml", UriKind.Relative);

            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private void LoadMainWindow()
        {
            if(window.IsLoaded)
                window.Show();
            Close();
        }

        private void Loader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Loader_CloseBtn(object sender, RoutedEventArgs e)
        {
            window.Close();
            Close();
        }

        private void StartProgressBar()
        {
            DoubleAnimation pbAnim = new()
            {
                From = -50,
                To = 100,
                DecelerationRatio = 0.4,
                Duration = TimeSpan.FromMilliseconds(3500)
            };
            pbAnim.Completed += PbAnim_Completed;
            PBar.BeginAnimation(ProgressBar.ValueProperty, pbAnim);
        }

        private void PbAnim_Completed(object? sender, EventArgs e)
        {
            SetTheme();
            DoubleAnimation closeUIHeightAnim = new()
            {
                From = UIWindow.ActualHeight,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(700)
            };
            UIWindow.BeginAnimation(Grid.HeightProperty, closeUIHeightAnim);

            DoubleAnimation closePBHeightAnim = new()
            {
                From = PBar.ActualHeight,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(700)
            };
            closePBHeightAnim.Completed += (s, e) => LoadMainWindow();
            PBar.BeginAnimation(ProgressBar.HeightProperty, closePBHeightAnim);

        }
    }
}
