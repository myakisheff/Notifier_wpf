namespace Notifier.domain
{
    internal class Task
    {
        public int id { get; set; }
        public string label { get; set; }
        public string text { get; set; }
        public string owner { get; set; }
        


        public Task(string label, string text) 
        {
            this.label = label;
            this.text = text;
        }

        public Task()
        {

        }

    }
}
