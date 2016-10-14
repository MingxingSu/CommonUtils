using System;
using System.Globalization;

namespace MaxSu.Framework.Common.Extensions
{
    public  static  class DbNullExtensions
    {
        public static object ParseIntDBNull(this String str)
        {
            if (str == "")
                return DBNull.Value;

            return int.Parse(str);
        }

        public static object ParseISODateDBNull(this String str)
        {
            if (str == "")
                return DBNull.Value;

            return DateTimeLibrary.ISODateToDateTime(str);
        }

        public static object ParseDoubleDBNull(this String str)
        {
            if (str == "")
                return DBNull.Value;

            CultureInfo culture = CultureInfo.InvariantCulture;
            string strReturnValue = str.Replace(",", ".");

            return double.Parse(strReturnValue, culture);
        }

        public static double ParseDoubleZero(this String str)
        {
            if (str == "")
                return 0.0;

            CultureInfo culture = CultureInfo.InvariantCulture;
            string strReturnValue = str.Replace(",", ".");

            return double.Parse(strReturnValue, culture);
        }

        public static object ParseStringDBNull(this String str)
        {
            if (str == "")
                return DBNull.Value;

            return str;
        }
    }
}
