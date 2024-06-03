namespace Notifier.domain
{
    internal class Task
    {
        public int id { get; set; } = 0;
        public string TaskTitle { get; set; } = "Title";
        public string TaskDescription { get; set; } = "Description";
        public string TaskCreationDate { get; set; } = "00.00.0000";
        public string TaskTargetDate { get; set; } = "00.00.0000";

    }
}
