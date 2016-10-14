using System;
using System.Security.Cryptography;
using System.Text;

namespace MaxSu.Framework.Common.Crypt
{
    /// <summary>
    ///     �õ������ȫ�루��ϣ���ܣ���
    /// </summary>
    public class HashEncode
    {
        /// <summary>
        ///     �õ������ϣ�����ַ���
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            string Security = HashEncoding(GetRandomValue());
            return Security;
        }

        /// <summary>
        ///     �õ�һ�������ֵ
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            var Seed = new Random();
            string RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }

        /// <summary>
        ///     ��ϣ����һ���ַ���
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        public static string HashEncoding(string Security)
        {
            byte[] Value;
            var Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(Security);
            var Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Security = "";
            foreach (byte o in Value)
            {
                Security += (int) o + "O";
            }
            return Security;
        }
    }
}