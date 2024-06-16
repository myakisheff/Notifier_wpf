using Notifier.ui;

namespace Notifier.domain.model
{
    internal class Task
    {
        public int? ID { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskDescription { get; set; }
        public string? TaskCreationDate { get; set; }
        public string? TaskTargetDate { get; set; }
        public List<DateInfo> TargetDateList { get; set; } = new();

        public DateTime? GetNearestDate()
        {
            if (TargetDateList == null || TargetDateList.Count == 0)
                return null;

            DateInfo? nearestDate = TargetDateList[0];

            foreach (DateInfo date in TargetDateList) 
            {
                DateTime datedt = DateTime.ParseExact($"{date.Date} {date.Time}", "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

                if (datedt < DateTime.Now)
                {
                    continue;
                }

                DateTime nearestDatedt = DateTime.ParseExact($"{nearestDate.Date} {nearestDate.Time}", "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

                if (nearestDatedt > datedt)
                    nearestDate = date;
            }

            return DateTime.ParseExact($"{nearestDate.Date} {nearestDate.Time}", "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
