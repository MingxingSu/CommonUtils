using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MaxSu.Framework.Common.Crypt
{
    /// <summary>
    ///     加密解密实用类。
    /// </summary>
    public class Encrypt
    {
        //密钥
        private static readonly byte[] arrDESKey = new byte[] {42, 16, 93, 156, 78, 4, 218, 32};
        private static readonly byte[] arrDESIV = new byte[] {55, 103, 246, 79, 36, 99, 167, 3};

        /// <summary>
        ///     加密。
        /// </summary>
        /// <param name="m_Need_Encode_String"></param>
        /// <returns></returns>
        public static string Encode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null)
            {
                throw new Exception("Error: \n源字符串为空！！");
            }
            var objDES = new DESCryptoServiceProvider();
            var objMemoryStream = new MemoryStream();
            var objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(arrDESKey, arrDESIV),
                                                   CryptoStreamMode.Write);
            var objStreamWriter = new StreamWriter(objCryptoStream);
            objStreamWriter.Write(m_Need_Encode_String);
            objStreamWriter.Flush();
            objCryptoStream.FlushFinalBlock();
            objMemoryStream.Flush();
            return Convert.ToBase64String(objMemoryStream.GetBuffer(), 0, (int) objMemoryStream.Length);
        }

        /// <summary>
        ///     解密。
        /// </summary>
        /// <param name="m_Need_Encode_String"></param>
        /// <returns></returns>
        public static string Decode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null)
            {
                throw new Exception("Error: \n源字符串为空！！");
            }
            var objDES = new DESCryptoServiceProvider();
            byte[] arrInput = Convert.FromBase64String(m_Need_Encode_String);
            var objMemoryStream = new MemoryStream(arrInput);
            var objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(arrDESKey, arrDESIV),
                                                   CryptoStreamMode.Read);
            var objStreamReader = new StreamReader(objCryptoStream);
            return objStreamReader.ReadToEnd();
        }

        /// <summary>
        ///     md5
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string Md5(string encypStr)
        {
            string retStr;
            var m5 = new MD5CryptoServiceProvider();
            byte[] inputBye;
            byte[] outputBye;
            inputBye = Encoding.ASCII.GetBytes(encypStr);
            outputBye = m5.ComputeHash(inputBye);
            retStr = Convert.ToBase64String(outputBye);
            return (retStr);
        }
    }
}