using System.Windows;

namespace Notifier.domain
{
    class TimeManager
    {
        public string CurrentTime { get; private set; } = "00:00";

        private bool IsTimeCorrect(string hour, string minute)
        {
            if(CheckHours(hour) && CheckMinutes(minute))
            {
                return true;
            }   
            return false;
        }

        public bool SetNewTime(string hour, string minute)
        {
            if (String.IsNullOrEmpty(minute))
                minute = "00";
            
            if (String.IsNullOrEmpty(hour))
                hour = "00";

            int newHour = Convert.ToInt32(hour);
            int newMinute = Convert.ToInt32(minute);

            string newHourStr = newHour.ToString();
            string newMinuteStr = newMinute.ToString();

            if (newHour < 10 && newHour >= 0)
                newHourStr = "0" + newHourStr;

            if (newMinute < 10 && newMinute >= 0)
                newMinuteStr = "0" + newMinuteStr;

            if (!IsTimeCorrect(newHourStr, newMinuteStr))
                return false;

            CurrentTime = $"{newHourStr}:{newMinuteStr}";

            return true;
        }

        public void AddHours(int hour)
        {
            int newHours = (Convert.ToInt32(GetHours()) + hour) % 24;
            SetNewTime(newHours.ToString(), GetMinutes());
        }

        public void AddMinutes(int minutes)
        {
            int newMinutes = (Convert.ToInt32(GetMinutes()) + minutes) % 60;
            SetNewTime(GetHours(), newMinutes.ToString());
        }

        public void RemoveHours(int hour)
        {
            int newHours = Convert.ToInt32(GetHours()) - hour;
            while (newHours < 0) newHours += 24;
            SetNewTime(newHours.ToString(), GetMinutes());
        }

        public void RemoveMinutes(int minutes)
        {
            int newMinutes = Convert.ToInt32(GetMinutes()) - minutes;
            while (newMinutes < 0) newMinutes += 60;
            SetNewTime(GetHours(), newMinutes.ToString());
        }

        public string GetHours()
        {
            return $"{CurrentTime[0]}{CurrentTime[1]}";
        }

        public string GetMinutes()
        {
            return $"{CurrentTime[3]}{CurrentTime[4]}";
        }

        private static bool CheckHours(string hour)
        {
            if (String.IsNullOrEmpty(hour))
                return false;

            if (Convert.ToInt32(hour) >= 0 && Convert.ToInt32(hour) < 24)
            {
                return true;
            }
            return false;
        }

        private static bool CheckMinutes(string minute)
        {
            if (String.IsNullOrEmpty(minute))
                return false;

            if (Convert.ToInt32(minute) >= 0 && Convert.ToInt32(minute) < 60)
            {
                return true;
            }
            return false;
        }
    }
}
