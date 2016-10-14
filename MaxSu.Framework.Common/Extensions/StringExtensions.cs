using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace MaxSu.Framework.Common.Extensions
{
    [StandardModule]
    public static class StringExtensions
    {
        public static string CombineUrl(this string baseUrl, string strPartToAdd)
        {
            string str2 = string.Empty;
            if ((baseUrl != null) && baseUrl.EndsWith("/"))
            {
                if ((strPartToAdd != null) && strPartToAdd.StartsWith("/"))
                {
                    strPartToAdd = strPartToAdd.Substring(1);
                }
            }
            else if ((strPartToAdd == null) || !strPartToAdd.StartsWith("/"))
            {
                str2 = "/";
            }
            return (baseUrl + str2 + strPartToAdd);
        }

        public static string ToLowerDateFormat(this string strBaseFormat)
        {
            if (strBaseFormat.Contains("D"))
            {
                strBaseFormat = strBaseFormat.Replace("D", "d");
            }
            if (strBaseFormat.Contains("Y"))
            {
                strBaseFormat = strBaseFormat.Replace("Y", "y");
            }
            if (strBaseFormat.Contains("S"))
            {
                strBaseFormat = strBaseFormat.Replace("S", "s");
            }
            if (strBaseFormat.Contains("H"))
            {
                strBaseFormat = strBaseFormat.Replace("H", "h");
            }
            if (strBaseFormat.Contains("T"))
            {
                strBaseFormat = strBaseFormat.Replace("T", "t");
            }
            return strBaseFormat;
        }

        public static bool AsBoolean(this string strValue)
        {
            if ((strValue != "1") && (strValue.ToUpper() != "TRUE"))
            {
                return false;
            }
            return true;
        }

        public static string GetFormatDatePart(this string strDateTimeFormat)
        {
            strDateTimeFormat = Regex.Replace(strDateTimeFormat, @"[^YyMDd\W]", "@");
            strDateTimeFormat = Regex.Replace(strDateTimeFormat, "@(.+)@", "");
            strDateTimeFormat = Regex.Replace(strDateTimeFormat, "@", "");
            return strDateTimeFormat;
        }

        public static string GetFormatTimePart(this string strDateTimeFormat)
        {
            strDateTimeFormat = Regex.Replace(strDateTimeFormat, @"[^HhmSsTt\W]", "@");
            strDateTimeFormat = Regex.Replace(strDateTimeFormat, "@(.+)@", "");
            strDateTimeFormat = Regex.Replace(strDateTimeFormat, "@", "");
            return strDateTimeFormat;
        }

        public static bool HasTimeFormat(this string baseFormat)
        {
            if ((!baseFormat.Contains("H") && !baseFormat.Contains("h")) &&
                ((!baseFormat.Contains("m") && !baseFormat.Contains("S")) && !baseFormat.Contains("s")))
            {
                return false;
            }
            return true;
        }
    }
}