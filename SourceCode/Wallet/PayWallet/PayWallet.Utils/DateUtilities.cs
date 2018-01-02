using System;

namespace PayWallet.Utils
{
    public class DateUtilities
    {


        /// <summary>
        /// Common DateTime Methods.
        /// </summary>
        /// 
        public enum Quarter
        {
            First = 1,
            Second = 2,
            Third = 3,
            Fourth = 4
        }

        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }


        #region Quarter

        public static DateTime GetStartOfQuarter(int year, Quarter qtr)
        {
            if (qtr == Quarter.First)	// 1st Quarter = January 1 to March 31
                return new DateTime(year, 1, 1, 0, 0, 0, 0);
            if (qtr == Quarter.Second) // 2nd Quarter = April 1 to June 30
                return new DateTime(year, 4, 1, 0, 0, 0, 0);
            if (qtr == Quarter.Third) // 3rd Quarter = July 1 to September 30
                return new DateTime(year, 7, 1, 0, 0, 0, 0);
            return new DateTime(year, 10, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfQuarter(int year, Quarter qtr)
        {
            if (qtr == Quarter.First)	// 1st Quarter = January 1 to March 31
                return new DateTime(year, 3, DateTime.DaysInMonth(year, 3), 23, 59, 59, 999);
            if (qtr == Quarter.Second) // 2nd Quarter = April 1 to June 30
                return new DateTime(year, 6, DateTime.DaysInMonth(year, 6), 23, 59, 59, 999);
            if (qtr == Quarter.Third) // 3rd Quarter = July 1 to September 30
                return new DateTime(year, 9, DateTime.DaysInMonth(year, 9), 23, 59, 59, 999);
            return new DateTime(year, 12, DateTime.DaysInMonth(year, 12), 23, 59, 59, 999);
        }

        public static Quarter GetQuarter(Month month)
        {
            if (month <= Month.March)	// 1st Quarter = January 1 to March 31
                return Quarter.First;
            if ((month >= Month.April) && (month <= Month.June)) // 2nd Quarter = April 1 to June 30
                return Quarter.Second;
            if ((month >= Month.July) && (month <= Month.September)) // 3rd Quarter = July 1 to September 30
                return Quarter.Third;
            return Quarter.Fourth;
        }

        public static DateTime GetEndOfLastQuarter()
        {
            return DateTime.Now.Month <= (int)Month.March ? GetEndOfQuarter(DateTime.Now.Year - 1, GetQuarter(Month.December)) : GetEndOfQuarter(DateTime.Now.Year, GetQuarter((Month)DateTime.Now.Month));
        }

        public static DateTime GetStartOfLastQuarter()
        {
            return DateTime.Now.Month <= 3 ? GetStartOfQuarter(DateTime.Now.Year - 1, GetQuarter(Month.December)) : GetStartOfQuarter(DateTime.Now.Year, GetQuarter((Month)DateTime.Now.Month));
        }

        public static DateTime GetStartOfCurrentQuarter()
        {
            return GetStartOfQuarter(DateTime.Now.Year, GetQuarter((Month)DateTime.Now.Month));
        }

        public static DateTime GetEndOfCurrentQuarter()
        {
            return GetEndOfQuarter(DateTime.Now.Year, GetQuarter((Month)DateTime.Now.Month));
        }
        #endregion

        #region Weeks
        public static DateTime GetStartOfLastWeek()
        {
            var daysToSubtract = (int)DateTime.Now.DayOfWeek + 7;
            var dt = DateTime.Now.Subtract(TimeSpan.FromDays(daysToSubtract));
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfLastWeek()
        {
            var dt = GetStartOfLastWeek().AddDays(6);
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }

        public static DateTime GetStartOfCurrentWeek()
        {
            var daysToSubtract = (int)DateTime.Now.DayOfWeek;
            DateTime dt = DateTime.Now.Subtract(TimeSpan.FromDays(daysToSubtract));
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfCurrentWeek()
        {
            var dt = GetStartOfCurrentWeek().AddDays(6);
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }
        #endregion

        #region Months

        public static DateTime GetStartOfMonth(int month, int year)
        {
            return new DateTime(year, month, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfMonth(int month, int year)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59, 999);
        }

        public static DateTime GetStartOfLastMonth()
        {
            return DateTime.Now.Month == 1 ? GetStartOfMonth(12, DateTime.Now.Year - 1) : GetStartOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
        }

        public static DateTime GetEndOfLastMonth()
        {
            return DateTime.Now.Month == 1 ? GetEndOfMonth(12, DateTime.Now.Year - 1) : GetEndOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
        }

        public static DateTime GetStartOfCurrentMonth()
        {
            return GetStartOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }

        public static DateTime GetEndOfCurrentMonth()
        {
            return GetEndOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }
        #endregion

        #region Years
        public static DateTime GetStartOfYear(int year)
        {
            return new DateTime(year, 1, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfYear(int year)
        {
            return new DateTime(year, 12, DateTime.DaysInMonth(year, 12), 23, 59, 59, 999);
        }

        public static DateTime GetStartOfLastYear()
        {
            return GetStartOfYear(DateTime.Now.Year - 1);
        }

        public static DateTime GetEndOfLastYear()
        {
            return GetEndOfYear(DateTime.Now.Year - 1);
        }

        public static DateTime GetStartOfCurrentYear()
        {
            return GetStartOfYear(DateTime.Now.Year);
        }

        public static DateTime GetEndOfCurrentYear()
        {
            return GetEndOfYear(DateTime.Now.Year);
        }
        #endregion

        #region Days

        public static DateTime GetStartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        #endregion


        #region[Convert]

        public static DateTime ConvertStringToDateTime(string date)
        {
            if (string.IsNullOrEmpty(date)) return DateTime.Now;
            var arr = date.Split('/');
            if (Config.GetCurrentLanguage().ToLower().Contains("en"))
                return new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));
            return new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[1]), Convert.ToInt32(arr[0]));
        }


        public static DateTime DateTimeExpress(DateTime dateTime, int hour, int minute, int second)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second);
        }

        public static string TimeAgo(DateTime dt, byte langId = 1)
        {
            TimeSpan span = DateTime.Now - dt;
            string result = string.Empty;

            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                result = langId == 1 ? String.Format("{0} năm trước",years) : years == 1 ? "a year ago" : String.Format("{0} years ago", years);
                return result;
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;

                result = langId == 1 ? String.Format("{0} tháng trước", months) : months == 1 ? "month ago" : String.Format("{0} months ago", months);
                return result;
            }
            if (span.Days > 0)
                return langId == 1 ? span.Days == 1 ? string.Format("Hôm qua lúc {0:H:mm}", dt) : String.Format("{0} tháng {1} lúc {2:H:mm}", dt.Day, dt.Month, dt) : span.Days == 1 ? string.Format("Yesterday at {0:hh:mm tt}", dt) : String.Format("{0} {1} at {2:hh:mm tt}", dt.ToString("MMM"), dt.Day, dt);
            if (span.Hours > 0)
                return langId == 1 ? String.Format("{0} giờ trước", span.Hours) : String.Format("{0} {1} ago", span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return langId == 1 ? String.Format("{0} phút trước", span.Minutes) : String.Format("{0} {1} ago", span.Minutes, span.Minutes == 1 ? "minute" : "minute");
            if (span.Seconds > 5)
                return langId == 1 ? String.Format("{0} giây trước", span.Seconds) : String.Format("{0} seconds ago", span.Seconds); 
            if (span.Seconds <= 5)
                return langId == 1 ? "Vừa xong" : "Just now";
            return string.Empty;
        }
        #endregion
    }
}


