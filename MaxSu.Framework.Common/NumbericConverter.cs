using System;
using System.Text;

namespace MaxSu.Framework.Common
{
    /// <summary>
    ///     处理数据类型转换，数制转换、编码转换相关的类
    /// </summary>
    public sealed class NumbericConverter
    {
        #region 补足位数

        /// <summary>
        ///     指定字符串的固定长度，如果字符串小于固定长度，
        ///     则在字符串的前面补足零，可设置的固定长度最大为9位
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="limitedLength">字符串的固定长度</param>
        public static string RepairZero(string text, int limitedLength)
        {
            //补足0的字符串
            string temp = "";

            //补足0
            for (int i = 0; i < limitedLength - text.Length; i++)
            {
                temp += "0";
            }

            //连接text
            temp += text;

            //返回补足0的字符串
            return temp;
        }

        #endregion

        #region 各进制数间转换

        /// <summary>
        ///     实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from); //先转成10进制
                string result = Convert.ToString(intValue, to); //再转成目标进制
                if (to == 2)
                {
                    int resultLength = result.Length; //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;
                        case 6:
                            result = "00" + result;
                            break;
                        case 5:
                            result = "000" + result;
                            break;
                        case 4:
                            result = "0000" + result;
                            break;
                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "0";
            }
        }

        #endregion

        #region 使用指定字符集将string转换成byte[]

        /// <summary>
        ///     使用指定字符集将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        #endregion

        #region 使用指定字符集将byte[]转换成string

        /// <summary>
        ///     使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        #endregion

        #region 将byte[]转换成int

        /// <summary>
        ///     将byte[]转换成int
        /// </summary>
        /// <param name="data">需要转换成整数的byte数组</param>
        public static int BytesToInt32(byte[] data)
        {
            //如果传入的字节数组长度小于4,则返回0
            if (data.Length < 4)
            {
                return 0;
            }

            //定义要返回的整数
            int num = 0;

            //如果传入的字节数组长度大于4,需要进行处理
            if (data.Length >= 4)
            {
                //创建一个临时缓冲区
                var tempBuffer = new byte[4];

                //将传入的字节数组的前4个字节复制到临时缓冲区
                Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                //将临时缓冲区的值转换成整数，并赋给num
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            //返回整数
            return num;
        }

        #endregion

        #region public static 找到最顶级的最大数字
        /// <summary>
        /// 找到最顶级的最大数字，如456-500
        /// </summary>
        /// <param name="OriginalNumber"></param>
        /// <returns></returns>
        public static int GetTopLevelMaxNumber(int OriginalNumber)
        {
            int intLength = 0;
            string strZero = "";
            double dblMiddleNumber = 0;

            intLength = OriginalNumber.ToString().Length;

            if (intLength == 1)
            {
                //一位数字
                return OriginalNumber;
            }

            for (int i = 0; i < intLength - 1; i++)
            {
                strZero = strZero + "0";
            }

            dblMiddleNumber = ((double)OriginalNumber) / double.Parse("1" + strZero);

            dblMiddleNumber = Math.Ceiling(dblMiddleNumber);

            dblMiddleNumber = dblMiddleNumber * double.Parse("1" + strZero);

            return (int)dblMiddleNumber;
        }
        #endregion     
    }
}