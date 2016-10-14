using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Web.Security;


namespace MaxSu.Framework.Common
{
    public sealed class StringLibrary
    {
        private StringLibrary()
        {
        }

        public static string ByteArrayToHexString(byte[] arrByte)
        {
            return Strings.Replace(BitConverter.ToString(arrByte), "-", "", 1, -1, CompareMethod.Binary);
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            byte[] buffer;
            try
            {
                if ((hexString.Length%2) != 0)
                {
                    throw new Exception("HexStringToByteArray: invalid argument length, must be a multiple of 2.");
                }
                var buffer2 =
                    (byte[]) Array.CreateInstance(typeof (byte), new[] {(long) Math.Round(((hexString.Length)/2.0))});
                long num2 = buffer2.Length - 1;
                for (long i = 0L; i <= num2; i += 1L)
                {
                    buffer2[(int) i] = Convert.ToByte(hexString.Substring((int) (i*2L), 2), 0x10);
                }
                buffer = buffer2;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return buffer;
        }

        public static string PadWithBackSlash(string strPath)
        {
            if (Conversions.ToString(strPath[strPath.Length - 1]) != @"\")
            {
                strPath = strPath + @"\";
            }
            return strPath;
        }

        public static string ReplaceParameters(string strString, Array arParameters)
        {
            string str;
            try
            {
                for (long i = arParameters.Length - 1; i >= 0L; i += -1L)
                {
                    strString = Strings.Replace(strString, "%%" + Conversions.ToString((i + 1L)),
                                                Conversions.ToString(NewLateBinding.LateIndexGet(arParameters,
                                                                                                 new object[] {i}, null)),
                                                1, -1, CompareMethod.Binary);
                }
                str = strString;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        public static string[] SplitWords(string strStringToSplit, Array arrSeparators)
        {
            string[] strArray;
            string str = "";
            try
            {
                var list = new ArrayList();
                string str2 = strStringToSplit;
                int num = 0;
                int length = str2.Length;
                while (num < length)
                {
                    char objValue = str2[num];
                    if (ArrayLibrary.IsInArray(objValue, arrSeparators))
                    {
                        list.Add(str);
                        str = "";
                    }
                    else
                    {
                        str = str + Conversions.ToString(objValue);
                    }
                    num++;
                }
                list.Add(str);
                strArray = (string[]) list.ToArray(typeof (string));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return strArray;
        }

        /// <summary>
        ///     把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            var list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }

        /// <summary>
        ///     把字符串转 按照, 分割 换为数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new[] { ',' });
        }

        /// <summary>
        ///     把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///     得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<int> list)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///     得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayValueStr(Dictionary<int, int> list)
        {
            var sb = new StringBuilder();
            foreach (var kvp in list)
            {
                sb.Append(kvp.Value + ",");
            }
            if (list.Count > 0)
            {
                return DelLastComma(sb.ToString());
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///     转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///     转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///     把字符串按照指定分隔符装成 List 去除重复
        /// </summary>
        /// <param name="o_str"></param>
        /// <param name="sepeater"></param>
        /// <returns></returns>
        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            var list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }

        /// <summary>
        ///     分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitstr"></param>
        /// <returns></returns>
        public static string[] SplitMulti(string str, string splitstr)
        {
            string[] strArray = null;
            if ((str != null) && (str != ""))
            {
                strArray = new Regex(splitstr).Split(str);
            }
            return strArray;
        }

        public static string SqlSafeString(string String, bool IsDel)
        {
            if (IsDel)
            {
                String = String.Replace("'", "");
                String = String.Replace("\"", "");
                return String;
            }
            String = String.Replace("'", "&#39;");
            String = String.Replace("\"", "&#34;");
            return String;
        }

        #region 获取正确的Id，如果不是正整数，返回0

        /// <summary>
        ///     获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="_value"></param>
        /// <returns>返回正确的整数ID，失败返回0</returns>
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }

        #endregion

        #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。

        /// <summary>
        ///     检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。(0除外)
        /// </summary>
        /// <param name="_value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }

        #endregion

        #region 快速验证一个字符串是否符合指定的正则表达式。

        /// <summary>
        ///     快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="_express">正则表达式的内容。</param>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            var myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }

        #endregion

        #region 将字符串样式转换为纯字符串

        /// <summary>
        ///     将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }

        #endregion

        #region 将字符串转换为新样式

        /// <summary>
        ///     将字符串转换为新样式
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="NewStyle"></param>
        /// <param name="SplitString"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }

        #endregion

        #region 删除最后一个字符之后的字符

        /// <summary>
        ///     删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        ///     删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        #endregion

            private static string De(string strCookie, int type)
    {
        string str;
        if ((type%2) == 0)
        {
            str = DeTransform1(strCookie);
        }
        else
        {
            str = DeTransform3(strCookie);
        }
        return Transform2(strCookie);
    }

    //转换字符(Reverse)
    public static string Decode(string str)
    {
        str = str.Replace("<br>", "\n");
        str = str.Replace("&gt;", ">");
        str = str.Replace("&lt;", "<");
        str = str.Replace("&nbsp;", " ");
        str = str.Replace("&quot;", "\"");
        return str;
    }

    //解密
    public static string Decrypt(string Passowrd)
    {
        return FormsAuthentication.Decrypt(Passowrd).Name;
    }

    //解密方式一
    public static string DeTransform1(string str)
    {
        int i = 0;
        var sb = new StringBuilder();
        foreach (char a in str)
        {
            switch ((i%6))
            {
                case 0:
                    sb.Append((char) (a - '\x0001'));
                    break;

                case 1:
                    sb.Append((char) (a - '\x0005'));
                    break;

                case 2:
                    sb.Append((char) (a - '\a'));
                    break;

                case 3:
                    sb.Append((char) (a - '\x0002'));
                    break;

                case 4:
                    sb.Append((char) (a - '\x0004'));
                    break;

                case 5:
                    sb.Append((char) (a - '\t'));
                    break;
            }
            i++;
        }
        return sb.ToString();
    }

    //加密方式三
    public static string DeTransform3(string str)
    {
        int i = 0;
        var sb = new StringBuilder();
        foreach (char a in str)
        {
            switch ((i%6))
            {
                case 0:
                    sb.Append((char) (a - '\x0003'));
                    break;

                case 1:
                    sb.Append((char) (a - '\x0006'));
                    break;

                case 2:
                    sb.Append((char) (a - '\b'));
                    break;

                case 3:
                    sb.Append((char) (a - '\a'));
                    break;

                case 4:
                    sb.Append((char) (a - '\x0005'));
                    break;

                case 5:
                    sb.Append((char) (a - '\x0002'));
                    break;
            }
            i++;
        }
        return sb.ToString();
    }

    private static string En(string strCookie, int type)
    {
        string str;
        if ((type%2) == 0)
        {
            str = Transform1(strCookie);
        }
        else
        {
            str = Transform3(strCookie);
        }
        return Transform2(strCookie);
    }

    //转换字符
    public static string Encode(string str)
    {
        str = str.Replace("&", "&amp;");
        str = str.Replace("'", "''");
        str = str.Replace("\"", "&quot;");
        str = str.Replace(" ", "&nbsp;");
        str = str.Replace("<", "&lt;");
        str = str.Replace(">", "&gt;");
        str = str.Replace("\n", "<br>");
        return str;
    }

    //加密
    public static string Encrypt(string Password)
    {
        var ticket = new FormsAuthenticationTicket(Password, true, 2);
        return FormsAuthentication.Encrypt(ticket);
    }

    //SHA1加密,MD5加密
    public static string Encrypt(string Password, int Format)
    {
        switch (Format)
        {
            case 0:
                return FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");

            case 1:
                return FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");
        }
        return "";
    }

    public static string EncryptCookie(string strCookie, int type)
    {
        string str = En(strCookie, type);
        var sb = new StringBuilder();
        foreach (char a in str)
        {
            sb.Append(Convert.ToString(a, 0x10).PadLeft(4, '0'));
        }
        return sb.ToString();
    }

    //反转字符
    public static string Reverse(string str)
    {
        var sb = new StringBuilder();
        for (int i = str.Length - 1; i >= 0; i--)
        {
            sb.Append(str[i]);
        }
        return sb.ToString();
    }

    //解密方式一
    public static string Transform1(string str)
    {
        int i = 0;
        var sb = new StringBuilder();
        foreach (char a in str)
        {
            switch ((i%6))
            {
                case 0:
                    sb.Append((char) (a + '\x0001'));
                    break;

                case 1:
                    sb.Append((char) (a + '\x0005'));
                    break;

                case 2:
                    sb.Append((char) (a + '\a'));
                    break;

                case 3:
                    sb.Append((char) (a + '\x0002'));
                    break;

                case 4:
                    sb.Append((char) (a + '\x0004'));
                    break;

                case 5:
                    sb.Append((char) (a + '\t'));
                    break;
            }
            i++;
        }
        return sb.ToString();
    }

    public static string Transform2(string str)
    {
        uint j = 0;
        var sb = new StringBuilder();
        str = Reverse(str);
        foreach (char a in str)
        {
            j = a;
            if (j > 0xff)
            {
                j = (uint) ((a >> 8) + ((a & 0xff) << 8));
            }
            else
            {
                j = (uint) ((a >> 4) + ((a & 15) << 4));
            }
            sb.Append((char) j);
        }
        return sb.ToString();
    }

    //解密方式三
    public static string Transform3(string str)
    {
        int i = 0;
        var sb = new StringBuilder();
        foreach (char a in str)
        {
            switch ((i%6))
            {
                case 0:
                    sb.Append((char) (a + '\x0003'));
                    break;

                case 1:
                    sb.Append((char) (a + '\x0006'));
                    break;

                case 2:
                    sb.Append((char) (a + '\b'));
                    break;

                case 3:
                    sb.Append((char) (a + '\a'));
                    break;

                case 4:
                    sb.Append((char) (a + '\x0005'));
                    break;

                case 5:
                    sb.Append((char) (a + '\x0002'));
                    break;
            }
            i++;
        }
        return sb.ToString();
    }

    public static string RandString()
    {
        string str = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~!@#$%^&*,.:;'";
        var r = new Random();
        string result = string.Empty;

        for (int i = 0; i < 32; i++)
        {
            int m = r.Next(0, 76);
            string s = str.Substring(m, 1);
            result += s;
        }

        return result;
    }

    public static string DisGuise()
    {
        string str = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var r = new Random();
        string result = string.Empty;

        for (int i = 0; i < 32; i++)
        {
            int m = r.Next(0, 46);
            string s = str.Substring(m, 1);
            result += s;
        }

        return result;
    }
    }
}