using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using MaxSu.Framework.Common.Web;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MaxSu.Framework.Common
{
    public class FileManager
    {
        #region ���캯��

        private static string strRootFolder;

        static FileManager()
        {
            strRootFolder = HttpContext.Current.Request.PhysicalApplicationPath + "File\\";
            strRootFolder = strRootFolder.Substring(0, strRootFolder.LastIndexOf(@"\"));
        }

        #endregion

        #region ���ָ��Ŀ¼�Ƿ����

        /// <summary>
        ///     ���ָ��Ŀ¼�Ƿ����
        /// </summary>
        /// <param name="directoryPath">Ŀ¼�ľ���·��</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        #endregion

        #region ���ָ���ļ��Ƿ����,������ڷ���true

        /// <summary>
        ///     ���ָ���ļ��Ƿ����,��������򷵻�true��
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        #endregion

        #region ��ȡָ��Ŀ¼�е��ļ��б�

        /// <summary>
        ///     ��ȡָ��Ŀ¼�������ļ��б�
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        public static string[] GetFileNames(string directoryPath)
        {
            //���Ŀ¼�����ڣ����׳��쳣
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //��ȡ�ļ��б�
            return Directory.GetFiles(directoryPath);
        }

        #endregion

        #region ��ȡָ��Ŀ¼��������Ŀ¼�б�,��Ҫ����Ƕ�׵���Ŀ¼�б�,��ʹ�����ط���.

        /// <summary>
        ///     ��ȡָ��Ŀ¼��������Ŀ¼�б�,��Ҫ����Ƕ�׵���Ŀ¼�б�,��ʹ�����ط���.
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ��ȡָ��Ŀ¼����Ŀ¼�������ļ��б�

        /// <summary>
        ///     ��ȡָ��Ŀ¼����Ŀ¼�������ļ��б�
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        /// <param name="searchPattern">
        ///     ģʽ�ַ�����"*"����0��N���ַ���"?"����1���ַ���
        ///     ������"Log*.xml"��ʾ����������Log��ͷ��Xml�ļ���
        /// </param>
        /// <param name="isSearchChild">�Ƿ�������Ŀ¼</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //���Ŀ¼�����ڣ����׳��쳣
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ���ָ��Ŀ¼�Ƿ�Ϊ��

        /// <summary>
        ///     ���ָ��Ŀ¼�Ƿ�Ϊ��
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //�ж��Ƿ�����ļ�
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //�ж��Ƿ�����ļ���
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                //�����¼��־
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return true;
            }
        }

        #endregion

        #region ���ָ��Ŀ¼���Ƿ����ָ�����ļ�

        /// <summary>
        ///     ���ָ��Ŀ¼���Ƿ����ָ�����ļ�,��Ҫ������Ŀ¼��ʹ�����ط���.
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        /// <param name="searchPattern">
        ///     ģʽ�ַ�����"*"����0��N���ַ���"?"����1���ַ���
        ///     ������"Log*.xml"��ʾ����������Log��ͷ��Xml�ļ���
        /// </param>
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //��ȡָ�����ļ��б�
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //�ж�ָ���ļ��Ƿ����
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }

        /// <summary>
        ///     ���ָ��Ŀ¼���Ƿ����ָ�����ļ�
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        /// <param name="searchPattern">
        ///     ģʽ�ַ�����"*"����0��N���ַ���"?"����1���ַ���
        ///     ������"Log*.xml"��ʾ����������Log��ͷ��Xml�ļ���
        /// </param>
        /// <param name="isSearchChild">�Ƿ�������Ŀ¼</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //��ȡָ�����ļ��б�
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //�ж�ָ���ļ��Ƿ����
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }

        #endregion

        #region ����Ŀ¼

        /// <summary>
        ///     ����Ŀ¼
        /// </summary>
        /// <param name="dir">Ҫ������Ŀ¼·������Ŀ¼��</param>
        public static void CreateDir(string dir)
        {
            if (dir.Length == 0) return;
            if (!Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }

        #endregion

        #region ɾ��Ŀ¼

        /// <summary>
        ///     ɾ��Ŀ¼
        /// </summary>
        /// <param name="dir">Ҫɾ����Ŀ¼·��������</param>
        public static void DeleteDir(string dir)
        {
            if (dir.Length == 0) return;
            if (Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.Delete(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }

        #endregion

        #region ɾ���ļ�

        /// <summary>
        ///     ɾ���ļ�
        /// </summary>
        /// <param name="file">Ҫɾ�����ļ�·��������</param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + file))
                File.Delete(HttpContext.Current.Request.PhysicalApplicationPath + file);
        }

        #endregion

        #region �����ļ�

        /// <summary>
        ///     �����ļ�
        /// </summary>
        /// <param name="dir">����׺���ļ���</param>
        /// <param name="pagestr">�ļ�����</param>
        public static void CreateFile(string dir, string pagestr)
        {
            dir = dir.Replace("/", "\\");
            if (dir.IndexOf("\\") > -1)
                CreateDir(dir.Substring(0, dir.LastIndexOf("\\")));
            var sw = new StreamWriter(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir, false,
                                      Encoding.GetEncoding("GB2312"));
            sw.Write(pagestr);
            sw.Close();
        }

        #endregion

        #region �ƶ��ļ�(����--ճ��)

        /// <summary>
        ///     �ƶ��ļ�(����--ճ��)
        /// </summary>
        /// <param name="dir1">Ҫ�ƶ����ļ���·����ȫ��(������׺)</param>
        /// <param name="dir2">�ļ��ƶ����µ�λ��,��ָ���µ��ļ���</param>
        public static void MoveFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
                File.Move(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1,
                          HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2);
        }

        #endregion

        #region �����ļ�

        /// <summary>
        ///     �����ļ�
        /// </summary>
        /// <param name="dir1">Ҫ���Ƶ��ļ���·���Ѿ�ȫ��(������׺)</param>
        /// <param name="dir2">Ŀ��λ��,��ָ���µ��ļ���</param>
        public static void CopyFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
            {
                File.Copy(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1,
                          HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2, true);
            }
        }

        #endregion

        #region ����ʱ��õ�Ŀ¼�� / ��ʽ:yyyyMMdd ���� HHmmssff

        /// <summary>
        ///     ����ʱ��õ�Ŀ¼��yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        ///     ����ʱ��õ��ļ���HHmmssff
        /// </summary>
        /// <returns></returns>
        public static string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }

        #endregion

        #region �����ļ���

        /// <summary>
        ///     �����ļ���(�ݹ�)
        /// </summary>
        /// <param name="varFromDirectory">Դ�ļ���·��</param>
        /// <param name="varToDirectory">Ŀ���ļ���·��</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }
            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }

        #endregion

        #region ����ļ�,����ļ��������򴴽�

        /// <summary>
        ///     ����ļ�,����ļ��������򴴽�
        /// </summary>
        /// <param name="FilePath">·��,�����ļ���</param>
        public static void ExistsFile(string FilePath)
        {
            //if(!File.Exists(FilePath))    
            //File.Create(FilePath);    
            //����д���ᱨ��,��ϸ�����뿴����.........   
            if (!File.Exists(FilePath))
            {
                FileStream fs = File.Create(FilePath);
                fs.Close();
            }
        }

        #endregion

        #region ɾ��ָ���ļ��ж�Ӧ�����ļ�������ļ�

        /// <summary>
        ///     ɾ��ָ���ļ��ж�Ӧ�����ļ�������ļ�
        /// </summary>
        /// <param name="varFromDirectory">ָ���ļ���·��</param>
        /// <param name="varToDirectory">��Ӧ�����ļ���·��</param>
        public static void DeleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    DeleteFolderFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }


            string[] files = Directory.GetFiles(varFromDirectory);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Delete(varToDirectory + s.Substring(s.LastIndexOf("\\")));
                }
            }
        }

        #endregion

        #region ���ļ��ľ���·���л�ȡ�ļ���( ������չ�� )

        /// <summary>
        ///     ���ļ��ľ���·���л�ȡ�ļ���( ������չ�� )
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static string GetFileName(string filePath)
        {
            //��ȡ�ļ�������
            var fi = new FileInfo(filePath);
            return fi.Name;
        }

        #endregion

        /// <summary>
        ///     �����ļ��ο�����,ҳ��������
        /// </summary>
        /// <param name="cDir">��·��</param>
        /// <param name="TempId">ģ�������滻���</param>
        public static void CopyFiles(string cDir, string TempId)
        {
            //if (Directory.Exists(Request.PhysicalApplicationPath + "\\Controls"))
            //{
            //    string TempStr = string.Empty;
            //    StreamWriter sw;
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Default.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\List.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\View.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\DissList.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Diss.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Review.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Search.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //}
        }

        #region ����һ��Ŀ¼

        /// <summary>
        ///     ����һ��Ŀ¼
        /// </summary>
        /// <param name="directoryPath">Ŀ¼�ľ���·��</param>
        public static void CreateDirectory(string directoryPath)
        {
            //���Ŀ¼�������򴴽���Ŀ¼
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        #endregion

        #region ����һ���ļ�

        /// <summary>
        ///     ����һ���ļ���
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                //����ļ��������򴴽����ļ�
                if (!IsExistFile(filePath))
                {
                    //����һ��FileInfo����
                    var file = new FileInfo(filePath);

                    //�����ļ�
                    FileStream fs = file.Create();

                    //�ر��ļ���
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        ///     ����һ���ļ�,�����ֽ���д���ļ���
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        /// <param name="buffer">������������</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //����ļ��������򴴽����ļ�
                if (!IsExistFile(filePath))
                {
                    //����һ��FileInfo����
                    var file = new FileInfo(filePath);

                    //�����ļ�
                    FileStream fs = file.Create();

                    //д���������
                    fs.Write(buffer, 0, buffer.Length);

                    //�ر��ļ���
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }

        #endregion

        #region ��ȡ�ı��ļ�������

        /// <summary>
        ///     ��ȡ�ı��ļ�������
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static int GetLineCount(string filePath)
        {
            //���ı��ļ��ĸ��ж���һ���ַ���������
            string[] rows = File.ReadAllLines(filePath);

            //��������
            return rows.Length;
        }

        #endregion

        #region ��ȡһ���ļ��ĳ���

        /// <summary>
        ///     ��ȡһ���ļ��ĳ���,��λΪByte
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static int GetFileSize(string filePath)
        {
            //����һ���ļ�����
            var fi = new FileInfo(filePath);

            //��ȡ�ļ��Ĵ�С
            return (int)fi.Length;
        }

        #endregion

        #region ��ȡָ��Ŀ¼�е���Ŀ¼�б�

        /// <summary>
        ///     ��ȡָ��Ŀ¼����Ŀ¼��������Ŀ¼�б�
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        /// <param name="searchPattern">
        ///     ģʽ�ַ�����"*"����0��N���ַ���"?"����1���ַ���
        ///     ������"Log*.xml"��ʾ����������Log��ͷ��Xml�ļ���
        /// </param>
        /// <param name="isSearchChild">�Ƿ�������Ŀ¼</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ���ı��ļ���β��׷������

        /// <summary>
        ///     ���ı��ļ���β��׷������
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        /// <param name="content">д�������</param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }

        #endregion

        #region �������ļ������ݸ��Ƶ����ļ���

        /// <summary>
        ///     ��Դ�ļ������ݸ��Ƶ�Ŀ���ļ���
        /// </summary>
        /// <param name="sourceFilePath">Դ�ļ��ľ���·��</param>
        /// <param name="destFilePath">Ŀ���ļ��ľ���·��</param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }

        #endregion

        #region ���ļ��ƶ���ָ��Ŀ¼

        /// <summary>
        ///     ���ļ��ƶ���ָ��Ŀ¼
        /// </summary>
        /// <param name="sourceFilePath">��Ҫ�ƶ���Դ�ļ��ľ���·��</param>
        /// <param name="descDirectoryPath">�ƶ�����Ŀ¼�ľ���·��</param>
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //��ȡԴ�ļ�������
            string sourceFileName = GetFileName(sourceFilePath);

            if (IsExistDirectory(descDirectoryPath))
            {
                //���Ŀ���д���ͬ���ļ�,��ɾ��
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //���ļ��ƶ���ָ��Ŀ¼
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }

        #endregion

        #region ���ָ��Ŀ¼

        /// <summary>
        ///     ���ָ��Ŀ¼�������ļ�����Ŀ¼,����Ŀ¼��Ȼ����.
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                //ɾ��Ŀ¼�����е��ļ�
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }

                //ɾ��Ŀ¼�����е���Ŀ¼
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }

        #endregion

        #region ����ļ�����

        /// <summary>
        ///     ����ļ�����
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static void ClearFile(string filePath)
        {
            //ɾ���ļ�
            File.Delete(filePath);

            //���´������ļ�
            CreateFile(filePath);
        }

        #endregion

        #region ɾ��ָ��Ŀ¼

        /// <summary>
        ///     ɾ��ָ��Ŀ¼����������Ŀ¼
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        #endregion

        #region Ŀ¼

        /// <summary>
        ///     ����Ŀ¼
        /// </summary>
        public static string GetRootPath()
        {
            return strRootFolder;
        }

        /// <summary>
        ///     д��Ŀ¼
        /// </summary>
        public static void SetRootPath(string path)
        {
            strRootFolder = path;
        }

        /// <summary>
        ///     ��ȡĿ¼�б�
        /// </summary>
        public static List<FileItem> GetDirectoryItems()
        {
            return GetDirectoryItems(strRootFolder);
        }

        /// <summary>
        ///     ��ȡĿ¼�б�
        /// </summary>
        public static List<FileItem> GetDirectoryItems(string path)
        {
            var list = new List<FileItem>();
            string[] folders = Directory.GetDirectories(path);
            foreach (string s in folders)
            {
                var item = new FileItem();
                var di = new DirectoryInfo(s);
                item.Name = di.Name;
                item.FullName = di.FullName;
                item.CreationDate = di.CreationTime;
                item.IsFolder = false;
                list.Add(item);
            }
            return list;
        }

        #endregion

        #region �ļ�

        /// <summary>
        ///     ��ȡ�ļ��б�
        /// </summary>
        public static List<FileItem> GetFileItems()
        {
            return GetFileItems(strRootFolder);
        }

        /// <summary>
        ///     ��ȡ�ļ��б�
        /// </summary>
        public static List<FileItem> GetFileItems(string path)
        {
            var list = new List<FileItem>();
            string[] files = Directory.GetFiles(path);
            foreach (string s in files)
            {
                var item = new FileItem();
                var fi = new FileInfo(s);
                item.Name = fi.Name;
                item.FullName = fi.FullName;
                item.CreationDate = fi.CreationTime;
                item.IsFolder = true;
                item.Size = fi.Length;
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        ///     �����ļ�
        /// </summary>
        public static bool CreateFile(string filename, string path, byte[] contents)
        {
            try
            {
                FileStream fs = File.Create(path + "\\" + filename);
                fs.Write(contents, 0, contents.Length);
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     ��ȡ�ļ�
        /// </summary>
        public static string OpenText(string parentName)
        {
            StreamReader sr = File.OpenText(parentName);
            var output = new StringBuilder();
            string rl;
            while ((rl = sr.ReadLine()) != null)
            {
                output.Append(rl);
            }
            sr.Close();
            return output.ToString();
        }

        /// <summary>
        ///     ��ȡ�ļ���Ϣ
        /// </summary>
        public static FileItem GetItemInfo(string path)
        {
            var item = new FileItem();
            if (Directory.Exists(path))
            {
                var di = new DirectoryInfo(path);
                item.Name = di.Name;
                item.FullName = di.FullName;
                item.CreationDate = di.CreationTime;
                item.IsFolder = true;
                item.LastAccessDate = di.LastAccessTime;
                item.LastWriteDate = di.LastWriteTime;
                item.FileCount = di.GetFiles().Length;
                item.SubFolderCount = di.GetDirectories().Length;
            }
            else
            {
                var fi = new FileInfo(path);
                item.Name = fi.Name;
                item.FullName = fi.FullName;
                item.CreationDate = fi.CreationTime;
                item.LastAccessDate = fi.LastAccessTime;
                item.LastWriteDate = fi.LastWriteTime;
                item.IsFolder = false;
                item.Size = fi.Length;
            }
            return item;
        }

        /// <summary>
        ///     д��һ�����ļ������ļ���д�����ݣ�Ȼ��ر��ļ������Ŀ���ļ��Ѵ��ڣ����д���ļ���
        /// </summary>
        public static bool WriteAllText(string parentName, string contents)
        {
            try
            {
                File.WriteAllText(parentName, contents, Encoding.Unicode);
                return true;
            }
            catch
            {
                return false;
            }
        }
    
        #endregion

        #region ���ļ��ľ���·���л�ȡ�ļ���( ��������չ�� )

        /// <summary>
        ///     ���ļ��ľ���·���л�ȡ�ļ���( ��������չ�� )
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static string GetFileNameNoExtension(string filePath)
        {
            //��ȡ�ļ�������
            var fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }

        #endregion

        #region ���ļ��ľ���·���л�ȡ��չ��

        /// <summary>
        ///     ���ļ��ľ���·���л�ȡ��չ��
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static string GetExtension(string filePath)
        {
            //��ȡ�ļ�������
            var fi = new FileInfo(filePath);
            return fi.Extension;
        }

        #endregion

        #region �ļ���

        /// <summary>
        ///     �����ļ���
        /// </summary>
        public static void CreateFolder(string name, string parentName)
        {
            var di = new DirectoryInfo(parentName);
            di.CreateSubdirectory(name);
        }

        /// <summary>
        ///     �ƶ��ļ���
        /// </summary>
        public static bool MoveFolder(string oldPath, string newPath)
        {
            try
            {
                Directory.Move(oldPath, newPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ����ļ�

        /// <summary>
        ///     �ж��Ƿ�Ϊ��ȫ�ļ���
        /// </summary>
        /// <param name="str">�ļ���</param>
        public static bool IsSafeName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] arrExtension =
                {
                    ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap", ".jpg", ".gif", ".png",
                    ".rar", ".zip"
                };
            for (int i = 0; i < arrExtension.Length; i++)
            {
                if (strExtension.Equals(arrExtension[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     �ж��Ƿ�Ϊ����ȫ�ļ���
        /// </summary>
        /// <param name="str">�ļ������ļ�����</param>
        public static bool IsUnsafeName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] arrExtension =
                {
                    ".", ".asp", ".aspx", ".cs", ".net", ".dll", ".config", ".ascx", ".master", ".asmx",
                    ".asax", ".cd", ".browser", ".rpt", ".ashx", ".xsd", ".mdf", ".resx", ".xsd"
                };
            for (int i = 0; i < arrExtension.Length; i++)
            {
                if (strExtension.Equals(arrExtension[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     �ж��Ƿ�Ϊ�ɱ༭�ļ�
        /// </summary>
        /// <param name="str">�ļ������ļ�����</param>
        public static bool IsCanEdit(string strExtension)
        {
            strExtension = strExtension.ToLower();

            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] arrExtension = {".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap"};
            for (int i = 0; i < arrExtension.Length; i++)
            {
                if (strExtension.Equals(arrExtension[i]))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region �ڵ�ǰĿ¼�´���Ŀ¼

        /****************************************
         * �������ƣ�FolderCreate
         * ����˵�����ڵ�ǰĿ¼�´���Ŀ¼
         * ��    ����OrignFolder:��ǰĿ¼,NewFloder:��Ŀ¼
         * ����ʾ�У�
         *           string OrignFolder = Server.MapPath("test/");    
         *           string NewFloder = "new";
         *           MaxSu.Framework.Common.FileOperate.FolderCreate(OrignFolder, NewFloder); 
        *****************************************/

        /// <summary>
        ///     �ڵ�ǰĿ¼�´���Ŀ¼
        /// </summary>
        /// <param name="OrignFolder">��ǰĿ¼</param>
        /// <param name="NewFloder">��Ŀ¼</param>
        public static void FolderCreate(string OrignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(OrignFolder);
            Directory.CreateDirectory(NewFloder);
        }

        /// <summary>
        ///     �����ļ���
        /// </summary>
        /// <param name="Path"></param>
        public static void FolderCreate(string Path)
        {
            // �ж�Ŀ��Ŀ¼�Ƿ����������������½�֮
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        #endregion

        #region �ݹ�ɾ���ļ���Ŀ¼���ļ�

        /****************************************
         * �������ƣ�DeleteFolder
         * ����˵�����ݹ�ɾ���ļ���Ŀ¼���ļ�
         * ��    ����dir:�ļ���·��
         * ����ʾ�У�
         *           string dir = Server.MapPath("test/");  
         *           MaxSu.Framework.Common.FileOperate.DeleteFolder(dir);       
        *****************************************/

        /// <summary>
        ///     �ݹ�ɾ���ļ���Ŀ¼���ļ�
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //�����������ļ���ɾ��֮ 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //ֱ��ɾ�����е��ļ�                        
                    else
                        DeleteFolder(d); //�ݹ�ɾ�����ļ��� 
                }
                Directory.Delete(dir, true); //ɾ���ѿ��ļ���                 
            }
        }

        #endregion

        #region ��ָ���ļ����������������copy��Ŀ���ļ������� ��Ŀ���ļ���Ϊֻ�����Ծͻᱨ��

        /****************************************
         * �������ƣ�CopyDir
         * ����˵������ָ���ļ����������������copy��Ŀ���ļ������� ��Ŀ���ļ���Ϊֻ�����Ծͻᱨ��
         * ��    ����srcPath:ԭʼ·��,aimPath:Ŀ���ļ���
         * ����ʾ�У�
         *           string srcPath = Server.MapPath("test/");  
         *           string aimPath = Server.MapPath("test1/");
         *           MaxSu.Framework.Common.FileOperate.CopyDir(srcPath,aimPath);   
        *****************************************/

        /// <summary>
        ///     ָ���ļ����������������copy��Ŀ���ļ�������
        /// </summary>
        /// <param name="srcPath">ԭʼ·��</param>
        /// <param name="aimPath">Ŀ���ļ���</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // ���Ŀ��Ŀ¼�Ƿ���Ŀ¼�ָ��ַ�����������������֮
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // �ж�Ŀ��Ŀ¼�Ƿ����������������½�֮
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // �õ�ԴĿ¼���ļ��б��������ǰ����ļ��Լ�Ŀ¼·����һ������
                //�����ָ��copyĿ���ļ�������ļ���������Ŀ¼��ʹ������ķ���
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //�������е��ļ���Ŀ¼
                foreach (string file in fileList)
                {
                    //�ȵ���Ŀ¼��������������Ŀ¼�͵ݹ�Copy��Ŀ¼������ļ�

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                        //����ֱ��Copy�ļ�
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }

        #endregion

        #region ��ȡָ���ļ�����������Ŀ¼���ļ�(����)

        /****************************************
         * �������ƣ�GetFoldAll(string Path)
         * ����˵������ȡָ���ļ�����������Ŀ¼���ļ�(����)
         * ��    ����Path:��ϸ·��
         * ����ʾ�У�
         *           string strDirlist = Server.MapPath("templates");       
         *           this.Literal1.Text = MaxSu.Framework.Common.FileOperate.GetFoldAll(strDirlist);  
        *****************************************/

        /// <summary>
        ///     ��ȡָ���ļ�����������Ŀ¼���ļ�
        /// </summary>
        /// <param name="Path">��ϸ·��</param>
        public static string GetFoldAll(string Path)
        {
            string str = "";
            var thisOne = new DirectoryInfo(Path);
            str = ListTreeShowAsHtml(thisOne, 0, str);
            return str;
        }

        /// <summary>
        ///     ��ȡָ���ļ�����������Ŀ¼���ļ�����
        /// </summary>
        /// <param name="theDir">ָ��Ŀ¼</param>
        /// <param name="nLevel">Ĭ����ʼֵ,����ʱ,һ��Ϊ0</param>
        /// <param name="Rn">���ڵ��ӵĴ���ֵ,һ��Ϊ��</param>
        /// <returns></returns>
        public static string ListTreeShowAsHtml(DirectoryInfo theDir, int nLevel, string Rn) //�ݹ�Ŀ¼ �ļ�
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories(); //���Ŀ¼
            foreach (DirectoryInfo dirinfo in subDirectories)
            {
                if (nLevel == 0)
                {
                    Rn += "��";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "��&nbsp;";
                    }
                    Rn += _s + "��";
                }
                Rn += "<b>" + dirinfo.Name + "</b><br />";
                FileInfo[] fileInfo = dirinfo.GetFiles(); //Ŀ¼�µ��ļ�
                foreach (FileInfo fInfo in fileInfo)
                {
                    if (nLevel == 0)
                    {
                        Rn += "��&nbsp;��";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "��&nbsp;";
                        }
                        Rn += _f + "��&nbsp;��";
                    }
                    Rn += fInfo.Name + " <br />";
                }
                Rn = ListTreeShowAsHtml(dirinfo, nLevel + 1, Rn);
            }
            return Rn;
        }


        /****************************************
     * �������ƣ�GetFoldAll(string Path)
     * ����˵������ȡָ���ļ�����������Ŀ¼���ļ�(��������)
     * ��    ����Path:��ϸ·��
     * ����ʾ�У�
     *            string strDirlist = Server.MapPath("templates");      
     *            this.Literal2.Text = MaxSu.Framework.Common.FileOperate.GetFoldAll(strDirlist,"tpl","");
    *****************************************/

        /// <summary>
        ///     ��ȡָ���ļ�����������Ŀ¼���ļ�(��������)
        /// </summary>
        /// <param name="Path">��ϸ·��</param>
        /// <param name="DropName">�����б�����</param>
        /// <param name="tplPath">Ĭ��ѡ��ģ������</param>
        public static string GetFoldAllAsHtml(string Path, string DropName, string tplPath)
        {
            string strDrop = "<select name=\"" + DropName + "\" id=\"" + DropName +
                             "\"><option value=\"\">--��ѡ����ϸģ��--</option>";
            string str = "";
            var thisOne = new DirectoryInfo(Path);
            str = ListTreeShowAsHtml(thisOne, 0, str, tplPath);
            return strDrop + str + "</select>";
        }

        /// <summary>
        ///     ��ȡָ���ļ�����������Ŀ¼���ļ�����
        /// </summary>
        /// <param name="theDir">ָ��Ŀ¼</param>
        /// <param name="nLevel">Ĭ����ʼֵ,����ʱ,һ��Ϊ0</param>
        /// <param name="Rn">���ڵ��ӵĴ���ֵ,һ��Ϊ��</param>
        /// <param name="tplPath">Ĭ��ѡ��ģ������</param>
        /// <returns></returns>
        public static string ListTreeShowAsHtml(DirectoryInfo theDir, int nLevel, string Rn, string tplPath) //�ݹ�Ŀ¼ �ļ�
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories(); //���Ŀ¼

            foreach (DirectoryInfo dirinfo in subDirectories)
            {
                Rn += "<option value=\"" + dirinfo.Name + "\"";
                if (tplPath.ToLower() == dirinfo.Name.ToLower())
                {
                    Rn += " selected ";
                }
                Rn += ">";

                if (nLevel == 0)
                {
                    Rn += "��";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "��&nbsp;";
                    }
                    Rn += _s + "��";
                }
                Rn += "" + dirinfo.Name + "</option>";


                FileInfo[] fileInfo = dirinfo.GetFiles(); //Ŀ¼�µ��ļ�
                foreach (FileInfo fInfo in fileInfo)
                {
                    Rn += "<option value=\"" + dirinfo.Name + "/" + fInfo.Name + "\"";
                    if (tplPath.ToLower() == fInfo.Name.ToLower())
                    {
                        Rn += " selected ";
                    }
                    Rn += ">";

                    if (nLevel == 0)
                    {
                        Rn += "��&nbsp;��";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "��&nbsp;";
                        }
                        Rn += _f + "��&nbsp;��";
                    }
                    Rn += fInfo.Name + "</option>";
                }
                Rn = ListTreeShowAsHtml(dirinfo, nLevel + 1, Rn, tplPath);
            }
            return Rn;
        }

        #endregion

        #region ��ȡ�ļ��д�С

        /****************************************
         * �������ƣ�GetDirectoryLength(string dirPath)
         * ����˵������ȡ�ļ��д�С
         * ��    ����dirPath:�ļ�����ϸ·��
         * ����ʾ�У�
         *           string Path = Server.MapPath("templates"); 
         *           Response.Write(MaxSu.Framework.Common.FileOperate.GetDirectoryLength(Path));       
        *****************************************/

        /// <summary>
        ///     ��ȡ�ļ��д�С
        /// </summary>
        /// <param name="dirPath">�ļ���·��</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            var di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }

        #endregion

        #region ��ȡָ���ļ���ϸ����

        /****************************************
         * �������ƣ�GetFileAttibe(string filePath)
         * ����˵������ȡָ���ļ���ϸ����
         * ��    ����filePath:�ļ���ϸ·��
         * ����ʾ�У�
         *           string file = Server.MapPath("robots.txt");  
         *            Response.Write(MaxSu.Framework.Common.FileOperate.GetFileAttibe(file));         
        *****************************************/

        /// <summary>
        ///     ��ȡָ���ļ���ϸ����
        /// </summary>
        /// <param name="filePath">�ļ���ϸ·��</param>
        /// <returns></returns>
        public static SortedDictionary<string,string> GetFileAttibeAsHtml(string filePath)
        {
            var dict = new SortedDictionary<string, string>();
            var objFI = new FileInfo(filePath);

            foreach (var t in objFI.GetType().GetProperties())
            {
                dict.Add(t.Name,t.GetConstantValue().ToString());
            }
            return dict;
        }

        #endregion


        //GA's 
        public static void AppendToFile(string strPath, string strContent)
        {
            FileStream stream = null;
            StreamWriter writer = null;
            try
            {
                stream = new FileStream(strPath, FileMode.Append, FileAccess.Write);
                writer = new StreamWriter(stream);
                writer.Write(strContent);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public static string ExtractFileName(string strFullPath)
        {
            string str;
            try
            {
                str = Strings.Mid(strFullPath, Strings.InStrRev(strFullPath, @"\", -1, CompareMethod.Binary) + 1);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        public static string ExtractPath(string strFullPath)
        {
            string str;
            try
            {
                int length = strFullPath.LastIndexOf(@"\");
                str = strFullPath.Substring(0, length);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        public static string PadBackSlash(string strPath)
        {
            string str;
            try
            {
                if (Conversions.ToString(strPath[strPath.Length - 1]) != @"\")
                {
                    strPath = strPath + @"\";
                }
                str = strPath;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        public static string ReadFileContent(string strPath)
        {
            FileStream stream = null;
            StreamReader reader = null;
            string str;
            try
            {
                stream = new FileStream(strPath, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(stream);
                str = reader.ReadToEnd();
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return str;
        }

        public static void WriteFileContent(string strPath, string strContent)
        {
            FileStream stream = null;
            StreamWriter writer = null;
            try
            {
                stream = new FileStream(strPath, FileMode.Create, FileAccess.Write);
                writer = new StreamWriter(stream);
                writer.Write(strContent);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public static void WriteStreamToFile(Stream objStream, string strPath)
        {
            FileStream output = null;
            BinaryReader reader = null;
            BinaryWriter writer = null;
            try
            {
                objStream.Position = 0L;
                output = new FileStream(strPath, FileMode.Create, FileAccess.Write);
                reader = new BinaryReader(objStream);
                writer = new BinaryWriter(output);
                long length = objStream.Length;
                for (long i = 1L; i <= length; i += 1L)
                {
                    writer.Write(reader.ReadByte());
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
                if (output != null)
                {
                    output.Close();
                }
            }
        }
        /// <summary>
        ///
        /// </summary>
        public static byte[] GetTempDocumentContent(string strGUID, bool blnDeleteAfterRead)
        {

            FileStream objFileStream = null;
            BinaryReader objBinaryReader = null;

            byte[] bytArray = null;

            try
            {

                objFileStream = new FileStream(GetTempDocumentFileName(strGUID), FileMode.Open, FileAccess.Read);

                objBinaryReader = new BinaryReader(objFileStream);
                bytArray = objBinaryReader.ReadBytes((int)objFileStream.Length);

                objBinaryReader.Close();
                objFileStream.Close();

                if (blnDeleteAfterRead)
                {
                    try
                    {
                        File.Delete(GetTempDocumentFileName(strGUID));
                    }
                    catch (Exception)
                    {

                    }
                }

                return bytArray;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objBinaryReader.Close();
                objFileStream.Close();

            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string GetTempDocumentFileName(string strGUID)
        {
            try
            {

                return Path.Combine(System.Environment.GetEnvironmentVariable("TEMP"), strGUID);

            }
            catch (Exception)
            {
                throw;

            }
        }

        public static Stream LoadResource(string strResourceName)
        {
            Stream objResource = default(Stream);

            objResource = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().FullName.Split(',')[0] + "." + strResourceName);

            if (objResource == null)
            {
                throw new Exception("Resource " + strResourceName + " not found.");
            }

            return objResource;

        }

        public static string ImportFile(string strFileName, string strFilePath, bool bCheckExtension, string strFileExtension, string strBinaryFileString, Encoding objEncoding)
        {
            //throw new Exception("FILENAME - " + strFileName+"\r\n FILEPATH - "+ strFilePath);
            string strFullFileName;
            string strFullFileNamePath;

            // We verify the the uploaded file extension is of the correct type
            if (bCheckExtension)
            {
                if (!System.IO.Path.GetExtension(strFileName).ToLower().Equals(strFileExtension.ToLower()))
                    throw new ApplicationException("The uploaded file type is incorrect");
            }

            // We create the directory needed for the import if it is not already created
            System.IO.Directory.CreateDirectory(strFilePath);

            // We get the full file name and path
            strFullFileName = Guid.NewGuid() + "_" + System.IO.Path.GetFileName(strFileName);

            // We create the full file name and path
            strFullFileNamePath = strFilePath + "\\" + strFullFileName;

            // We create the file into which we will import the data and 
            // write the contents of the strBinaryString variable into it

            FileInfo fi = new FileInfo(@strFullFileNamePath);
            StreamWriter sw = new StreamWriter(fi.FullName, true, objEncoding);
            sw.Write(strBinaryFileString);
            sw.Close();

            return strFullFileName;
        }


    }
}