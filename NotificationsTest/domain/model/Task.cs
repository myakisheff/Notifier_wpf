using Notifier.ui;

namespace Notifier.domain.model
{
    internal class Task
    {
        public int ID { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskDescription { get; set; }
        public string? TaskCreationDate { get; set; }
        public string? TaskTargetDate { get; set; }
        public List<DateInfo> TargetDateList { get; set; } = new();

        public DateTime? GetNearestDate()
        {
            if (!TargetDateList.Any())
                return null;

            DateInfo? nearestDate = null;

            foreach (DateInfo date in TargetDateList) 
            {
                DateTime dateFromList = DateTime.ParseExact($"{date.Date} {date.Time}", "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

                if (dateFromList < DateTime.Now)
                {
                    continue;
                }

                if (nearestDate == null) nearestDate = date;
                else
                {
                    DateTime nearestDatedt = DateTime.ParseExact($"{nearestDate.Date} {nearestDate.Time}", "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

                    if (nearestDatedt > dateFromList)
                        nearestDate = date;
                }
            }

            if (nearestDate == null) return null;

            return DateTime.ParseExact($"{nearestDate.Date} {nearestDate.Time}", "dd.MM.yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
