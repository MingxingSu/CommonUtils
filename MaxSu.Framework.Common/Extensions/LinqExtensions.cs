using System.Linq;
using System.Reflection;
using System.Text;

namespace MaxSu.Framework.Common.Extensions
{
    /// <summary>
    ///     LinQ  Extentensions
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        ///     Converts the Linq data to a commaseperated string including header.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string ToCSVString(this IOrderedQueryable data)
        {
            return ToCSVString(data, ", ");
        }

        /// <summary>
        ///     Converts the Linq data to a commaseperated string including header.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string ToCSVString(this IOrderedQueryable data, string delimiter)
        {
            return ToCSVString(data, delimiter, null);
        }

        /// <summary>
        ///     Converts the Linq data to a commaseperated string including header.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="nullvalue">The nullvalue.</param>
        /// <returns></returns>
        public static string ToCSVString(this IOrderedQueryable data, string delimiter, string nullvalue)
        {
            var csvdata = new StringBuilder();
            string replaceFrom = delimiter.Trim();
            string replaceDelimiter = ";";
            PropertyInfo[] headers = data.ElementType.GetProperties();
            switch (replaceFrom)
            {
                case ";":
                    replaceDelimiter = ":";
                    break;
                case ",":
                    replaceDelimiter = "¸";
                    break;
                case "\t":
                    replaceDelimiter = "    ";
                    break;
            }
            if (headers.Length > 0)
            {
                foreach (PropertyInfo head in headers)
                {
                    csvdata.Append(head.Name.Replace("_", " ") + delimiter);
                }
                csvdata.Append("\n");
            }
            foreach (object row in data)
            {
                PropertyInfo[] fields = row.GetType().GetProperties();
                for (int i = 0; i < fields.Length; i++)
                {
                    object value = null;
                    try
                    {
                        value = fields[i].GetValue(row, null);
                    }
                    catch
                    {
                    }
                    if (value != null)
                    {
                        csvdata.Append(
                            value.ToString()
                                 .Replace("\r", "\f")
                                 .Replace("\n", " \f")
                                 .Replace("_", " ")
                                 .Replace(replaceFrom, replaceDelimiter) + delimiter);
                    }
                    else
                    {
                        csvdata.Append(nullvalue);
                        csvdata.Append(delimiter);
                    }
                }
                csvdata.Append("\n");
            }
            return csvdata.ToString();
        }
    }
}