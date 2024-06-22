using Microsoft.Toolkit.Uwp.Notifications;
using System.Globalization;
using System.Windows;
using Windows.Foundation.Collections;

namespace Notifier
{
    public partial class App : Application
    {
        private readonly static List<CultureInfo> m_Languages = new();

        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }

        public App()
        {
            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US"));
            m_Languages.Add(new CultureInfo("ru-RU"));
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string mutName = "Notifier";
            Mutex mutex = new(true, mutName, out bool createdNew);
            if (!createdNew)
            {
                Shutdown();
            }

            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                // Obtain the arguments from the notification
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                // Obtain any user input (text boxes, menu selections) from the notification
                ValueSet userInput = toastArgs.UserInput;

                // Need to dispatch to UI thread if performing UI operations
                Application.Current.Dispatcher.Invoke(delegate
                {
                    // TODO: Show the corresponding content
                    // MessageBox.Show("Toast activated. Args: " + toastArgs.Argument);
                });
            };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ToastNotificationManagerCompat.Uninstall();
            base.OnExit(e);
        }

        //public void SetAutoload(bool set)
        //{
        //    RegistryKey? key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);

        //    if (key is null) return;

        //    if (set)
        //    {
        //        key.SetValue("Notifier", "\"" + AppDomain.CurrentDomain.BaseDirectory + "Notifier.exe" + "\"");
        //    }
        //    else
        //    {
        //        key.DeleteValue("Notifier", false);
        //    }
        //    key.Close();
        //}
    }

}
