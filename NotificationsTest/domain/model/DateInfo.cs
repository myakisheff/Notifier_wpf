namespace Notifier.domain.model
{
    enum DateErrorType
    {
        None = 0,
        DateExists = 1,
        WrongDate = 2,

    }

    public class DateInfo
    {
        public int? ID { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
    }
}
