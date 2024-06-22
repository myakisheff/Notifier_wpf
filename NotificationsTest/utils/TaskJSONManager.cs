using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace Notifier.utils
{
    internal class TaskJSONManager
    {
        private static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//Tasks.json";

        public static void Write(List<domain.model.Task> tasks)
        {
            string json = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(path, json);
        }

        public static List<domain.model.Task>? Read()
        {
            string text;
            try
            {
                text = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                text = "";
            }

            return JsonConvert.DeserializeObject<List<domain.model.Task>>(text);
        }

        public static string GetPath() => path;
    }
}
