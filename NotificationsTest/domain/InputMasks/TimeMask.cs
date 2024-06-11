namespace Notifier.domain.InputMasks
{
    class TimeMask
    {
        public static (bool isSuccess, string text) MaskHM(string time) 
        {
            bool isSuccess = true;
            string resultTime = time;

            switch(resultTime.Length)
            {
                case 0:
                    break;
                case 1:
                    if (resultTime[0] == '0' || resultTime[0] == '1' || resultTime[0] == '2')
                        break;
                    else resultTime = '0' + resultTime + ':';
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    break;
            }

            return (isSuccess, resultTime);

            if (!IsTimeCorrect(time))
            {
                throw new Exception($"wrong format of {time}. Expected hh:mm");
            }

            int? hours = GetHours(time);

            if (hours.HasValue)
            {
                throw new Exception($"wrong hours format of {time}. Expected between 00 and 24");
            }

            int? minutes = GetMinutes(time);

            if (hours.HasValue)
            {
                throw new Exception($"wrong hours format of {time}. Expected between 00 and 24");
            }

            return (true, time);
        }

        public static string MaskHMS(string time)
        {
            if (!IsTimeCorrect(time))
            {
                throw new Exception($"wrong format of {time}. Expected hh:mm:ss");
            }

            return time;
        }

        private static int? GetHours(string time)
        {

            return 1;
        }

        private static int? GetMinutes(string time)
        {
            return 1;
        }

        private static int? GetSeconds(string time)
        {
            return 1;
        }

        private static bool IsTimeCorrect(string time) 
        { 
            if(time.Length == 5 && time[2] == ':')
            {
                return true;
            }
            else if(time.Length == 8 && time[2] == ':' && time[5] == ':')
            {
                return true;
            }

            return false;
        }
    }
}
