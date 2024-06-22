using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Resources;

namespace Notifier
{
    public partial class Loader : Window, INotifyPropertyChanged
    {
        private int _workerState;

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

            StreamResourceInfo sri = Application.GetResourceStream(
                new Uri("assets/cursors/cursor.cur", UriKind.Relative));
            Cursor customCursor = new(sri.Stream);
            UIWindow.Cursor = customCursor;

            CloseBtn.Click += Loader_CloseBtn;

            StartProgressBar();
        }

        private static void SetTheme()
        {
            var uri = new Uri("resourceDictionaries/themes/DarkTheme.xaml", UriKind.Relative);


            if (Application.LoadComponent(uri) is not ResourceDictionary resourceDict)
                return;

            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private void LoadMainWindow()
        {
            MainWindow window = new();
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
