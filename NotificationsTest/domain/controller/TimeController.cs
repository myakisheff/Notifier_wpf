using Notifier.domain.model;

namespace Notifier.domain.controller
{
    internal class TimeController
    {
        private readonly Time currentTime;

        public TimeController()
        {
            currentTime = new();
        }

        public string GetTimeString() => $"{GetHoursString()}:{GetMinutesString()}";

        public bool SetNewTime(string hour, string minute)
        {
            if(SetNewHour(hour) && SetNewMinute(minute))
            {
                return true;
            }
            return false;
        }

        public bool SetNewHour(string hour)
        {
            if (String.IsNullOrEmpty(hour))
                hour = "00";

            byte newHour = Convert.ToByte(hour);

            if (newHour < 0) newHour = 0;

            if (IsHourCorrect(newHour))
            {
                currentTime.Hour = newHour;
                return true;
            }
            return false;
        }

        public bool SetNewMinute(string minute)
        {
            if (String.IsNullOrEmpty(minute))
                minute = "00";

            byte newMinute = Convert.ToByte(minute);

            if (newMinute < 0) newMinute = 0;

            if (IsMinuteCorrect(newMinute))
            {
                currentTime.Minute = newMinute;
                return true;
            }
            return false;
        }

        public string GetHoursString()
        {
            string hour = currentTime.Hour.ToString();
            if (currentTime.Hour < 10) hour = $"0{hour}";
            return $"{hour}";
        }

        public string GetMinutesString()
        {
            string minute = currentTime.Minute.ToString();
            if (currentTime.Minute < 10) minute = $"0{minute}";
            return $"{minute}";
        }

        public void AddHours(int hours)
        {
            int newHours = (currentTime.Hour + hours) % 24;
            SetNewHour(newHours.ToString());
        }

        public void AddMinutes(int minutes)
        {
            int newMinutes = (currentTime.Minute + minutes) % 60;
            SetNewMinute(newMinutes.ToString());
        }

        public void RemoveHours(int hours)
        {
            int newHours = currentTime.Hour - hours;
            while (newHours < 0) newHours += 24;
            SetNewHour(newHours.ToString());
        }

        public void RemoveMinutes(int minutes)
        {
            int newMinutes = currentTime.Minute + minutes;
            while (newMinutes < 0) newMinutes += 60;
            SetNewMinute(newMinutes.ToString());
        }

        private static bool IsHourCorrect(byte hour)
        {
            if (hour >= 0 && hour < 24)
            {
                return true;
            }
            return false;
        }

        private static bool IsMinuteCorrect(byte minute)
        {
            if (minute >= 0 && minute < 60)
                return true;
            return false;
        }
    }
}
