namespace MaxSu.Framework.Common.Chinese
{
    /// <summary>
    ///     农历属性
    /// </summary>
    public class ChineseCalendarDate
    {
        /// <summary>
        ///     农历属象
        /// </summary>
        public string Animal = "";

        /// <summary>
        ///     阴历节日
        /// </summary>
        public string ChineseFestival = "";

        /// <summary>
        ///     阳历节日
        /// </summary>
        public string WesternFestival = "";

        /// <summary>
        ///     农历天(整型)
        /// </summary>
        public int cnIntDay = 0;

        /// <summary>
        ///     农历月份(整型)
        /// </summary>
        public int cnIntMonth = 0;

        /// <summary>
        ///     农历年(整型)
        /// </summary>
        public int cnIntYear = 0;

        /// <summary>
        ///     二十四节气
        /// </summary>
        public string cnSolarTerm = "";

        /// <summary>
        ///     农历天(字符)
        /// </summary>
        public string cnStrDay = "";

        /// <summary>
        ///     农历月份(字符)
        /// </summary>
        public string cnStrMonth = "";

        /// <summary>
        ///     农历年(支干)
        /// </summary>
        public string cnStrYear = "";
    }
}