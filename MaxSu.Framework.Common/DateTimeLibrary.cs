using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MaxSu.Framework.Common
{
    public sealed class DateTimeLibrary
    {
        private DateTimeLibrary()
        {
        }

        public static object DateTimeToISODate(DateTime datDateTime)
        {
            object obj2;
            try
            {
                obj2 = datDateTime.ToString("s");
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return obj2;
        }

        public static object ISODateToDateTime(object objISODate)
        {
            object obj2;
            try
            {
                DateTime time;
                if (Information.IsDBNull(RuntimeHelpers.GetObjectValue(objISODate)))
                {
                    return objISODate;
                }
                string s = Conversions.ToString(objISODate);
                if (DateTime.TryParse(s, out time))
                {
                    return time;
                }
                obj2 = DateTime.Parse(s.Replace(" ", "+"));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return obj2;
        }


        /// <summary>
        /// To avoid DateTime comparison faults in the database, date strings need to be formatted in the DMY format.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [Obsolete("Use DateTimeLibrary class instead")]
        public static string ConvertToDMYDateFormat(string date)
        {
            DateTime dateObject = DateTime.Parse(date, CultureInfo.CurrentCulture);
            return dateObject.ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        /// <summary>
        /// Convert source time to hub local time
        /// </summary>
        /// <param name="dtSourceTime"></param>
        /// <returns></returns>
        public static DateTime ConvertToHUBTime(DateTime dtSourceTime,string timeZoneName)
        {
            try
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                return TimeZoneInfo.ConvertTime(dtSourceTime, timeZoneInfo);
            }
            catch
            {
                throw;
            }
        }
    }
}