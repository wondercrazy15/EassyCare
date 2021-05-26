using System;
using System.Globalization;

namespace EasyCare.Core.Constants
{
    public static class DateTimeMyFormatExtensions
    {
        public static string ToMyFormatString(this DateTime dt)
        {
            CultureInfo culture = new CultureInfo("en-US");
            return dt.ToString("MM/dd/yyyy HH:mm:ss", culture);
        }
    }

    public static class StringMyDateTimeFormatExtension
    {
        public static DateTime ParseMyFormatDateTime(this string s)
        {
            CultureInfo culture = new CultureInfo("en-US");

            DateTime dateTimeObj = Convert.ToDateTime(s, culture);
            //var culture = System.Globalization.CultureInfo.CurrentCulture;
            return dateTimeObj;
            ////var culture = System.Globalization.CultureInfo.CurrentCulture;
            //return DateTime.ParseExact(s, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
        }
    }
}
