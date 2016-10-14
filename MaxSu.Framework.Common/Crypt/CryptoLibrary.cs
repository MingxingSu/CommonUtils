using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MaxSu.Framework.Common.Crypt
{
    public class CryptoLibrary
    {
        private CryptoLibrary()
        {
        }

        public static string Decrypt(string strTextToDecrypt, string strKey, string strHashAlgoritm = "TRIPLEDES")
        {
            string str;
            try
            {
                string str2 = Strings.UCase(strHashAlgoritm);
                if (str2 == "TRIPLEDES")
                {
                    return DecryptTripleDES(strTextToDecrypt, strKey);
                }
                if (str2 != "RIJNDAEL")
                {
                    throw new Exception(strHashAlgoritm + " algorithm is not supported.");
                }
                str = DecryptRIJNDAEL(strTextToDecrypt, strKey);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        private static string DecryptRIJNDAEL(string strText, string strKey)
        {
            string str;
            var provider = new MD5CryptoServiceProvider();
            try
            {
                ICryptoTransform transform =
                    new RijndaelManaged
                        {
                            Key = provider.ComputeHash(Encoding.ASCII.GetBytes(strKey)),
                            Mode = CipherMode.ECB
                        }.CreateDecryptor();
                byte[] inputBuffer = Convert.FromBase64String(strText);
                str = Encoding.ASCII.GetString(transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        private static string DecryptTripleDES(string strText, string strKey)
        {
            string str;
            var provider = new TripleDESCryptoServiceProvider();
            var provider2 = new MD5CryptoServiceProvider();
            try
            {
                provider.Key = provider2.ComputeHash(Encoding.ASCII.GetBytes(strKey));
                provider.Mode = CipherMode.ECB;
                ICryptoTransform transform = provider.CreateDecryptor();
                byte[] inputBuffer = Convert.FromBase64String(strText);
                str = Encoding.ASCII.GetString(transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        public static string Encrypt(string strTextToEncrypt, string strKey, string strHashAlgoritm = "TRIPLEDES")
        {
            string str;
            try
            {
                string str2 = Strings.UCase(strHashAlgoritm);
                if (str2 == "TRIPLEDES")
                {
                    return EncryptTripleDES(strTextToEncrypt, strKey);
                }
                if (str2 != "RIJNDAEL")
                {
                    throw new Exception(strHashAlgoritm + " algorithm is not supported.");
                }
                str = EncryptRIJNDAEL(strTextToEncrypt, strKey);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        private static string EncryptRIJNDAEL(string strTextToEncrypt, string strKey)
        {
            string str;
            var provider = new MD5CryptoServiceProvider();
            try
            {
                ICryptoTransform transform =
                    new RijndaelManaged
                        {
                            Key = provider.ComputeHash(Encoding.ASCII.GetBytes(strKey)),
                            Mode = CipherMode.ECB
                        }.CreateEncryptor();
                byte[] bytes = Encoding.ASCII.GetBytes(strTextToEncrypt);
                str = Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        private static string EncryptTripleDES(string strText, string strKey)
        {
            string str;
            var provider = new TripleDESCryptoServiceProvider();
            var provider2 = new MD5CryptoServiceProvider();
            try
            {
                provider.Key = provider2.ComputeHash(Encoding.ASCII.GetBytes(strKey));
                provider.Mode = CipherMode.ECB;
                ICryptoTransform transform = provider.CreateEncryptor();
                byte[] bytes = Encoding.ASCII.GetBytes(strText);
                str = Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        public static string HashEncrypt(string strToHash, string strHashAlgoritm)
        {
            string str;
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(strToHash.ToCharArray());
                var arrByte = new byte[0];
                if (Strings.UCase(strHashAlgoritm) == "SHA")
                {
                    arrByte = new SHA1CryptoServiceProvider().ComputeHash(bytes);
                }
                else
                {
                    arrByte = new MD5CryptoServiceProvider().ComputeHash(bytes);
                }
                str = StringLibrary.ByteArrayToHexString(arrByte);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }
    }
}