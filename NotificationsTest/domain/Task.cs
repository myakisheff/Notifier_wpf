using Notifier.ui;

namespace Notifier.domain
{
    internal class Task
    {
        public int? id { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskDescription { get; set; }
        public string? TaskCreationDate { get; set; }
        public List<DateInfo>? TargetDateList { get; set; } = new();

    }
}
