using Newtonsoft.Json;
using System.IO;

namespace Notifier.utils
{
    internal class TaskJSONManager
    {
        private static readonly string path = "Tasks.json";

        public static void Write(List<domain.model.Task> tasks)
        {
            string json = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(path, json);
        }

        public static List<domain.model.Task>? Read()
        {
            string objects = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<domain.model.Task>>(objects);
        }

        public static string GetPath() => path;
    }
}
