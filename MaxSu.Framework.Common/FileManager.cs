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
        #region 构造函数

        private static string strRootFolder;

        static FileManager()
        {
            strRootFolder = HttpContext.Current.Request.PhysicalApplicationPath + "File\\";
            strRootFolder = strRootFolder.Substring(0, strRootFolder.LastIndexOf(@"\"));
        }

        #endregion

        #region 检测指定目录是否存在

        /// <summary>
        ///     检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        #endregion

        #region 检测指定文件是否存在,如果存在返回true

        /// <summary>
        ///     检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        #endregion

        #region 获取指定目录中的文件列表

        /// <summary>
        ///     获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表
            return Directory.GetFiles(directoryPath);
        }

        #endregion

        #region 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.

        /// <summary>
        ///     获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
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

        #region 获取指定目录及子目录中所有文件列表

        /// <summary>
        ///     获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">
        ///     模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        ///     范例："Log*.xml"表示搜索所有以Log开头的Xml文件。
        /// </param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常
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

        #region 检测指定目录是否为空

        /// <summary>
        ///     检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                //这里记录日志
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return true;
            }
        }

        #endregion

        #region 检测指定目录中是否存在指定的文件

        /// <summary>
        ///     检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">
        ///     模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        ///     范例："Log*.xml"表示搜索所有以Log开头的Xml文件。
        /// </param>
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在
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
        ///     检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">
        ///     模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        ///     范例："Log*.xml"表示搜索所有以Log开头的Xml文件。
        /// </param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判断指定文件是否存在
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

        #region 创建目录

        /// <summary>
        ///     创建目录
        /// </summary>
        /// <param name="dir">要创建的目录路径包括目录名</param>
        public static void CreateDir(string dir)
        {
            if (dir.Length == 0) return;
            if (!Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }

        #endregion

        #region 删除目录

        /// <summary>
        ///     删除目录
        /// </summary>
        /// <param name="dir">要删除的目录路径和名称</param>
        public static void DeleteDir(string dir)
        {
            if (dir.Length == 0) return;
            if (Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.Delete(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }

        #endregion

        #region 删除文件

        /// <summary>
        ///     删除文件
        /// </summary>
        /// <param name="file">要删除的文件路径和名称</param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + file))
                File.Delete(HttpContext.Current.Request.PhysicalApplicationPath + file);
        }

        #endregion

        #region 创建文件

        /// <summary>
        ///     创建文件
        /// </summary>
        /// <param name="dir">带后缀的文件名</param>
        /// <param name="pagestr">文件内容</param>
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

        #region 移动文件(剪贴--粘贴)

        /// <summary>
        ///     移动文件(剪贴--粘贴)
        /// </summary>
        /// <param name="dir1">要移动的文件的路径及全名(包括后缀)</param>
        /// <param name="dir2">文件移动到新的位置,并指定新的文件名</param>
        public static void MoveFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
                File.Move(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1,
                          HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2);
        }

        #endregion

        #region 复制文件

        /// <summary>
        ///     复制文件
        /// </summary>
        /// <param name="dir1">要复制的文件的路径已经全名(包括后缀)</param>
        /// <param name="dir2">目标位置,并指定新的文件名</param>
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

        #region 根据时间得到目录名 / 格式:yyyyMMdd 或者 HHmmssff

        /// <summary>
        ///     根据时间得到目录名yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        ///     根据时间得到文件名HHmmssff
        /// </summary>
        /// <returns></returns>
        public static string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }

        #endregion

        #region 复制文件夹

        /// <summary>
        ///     复制文件夹(递归)
        /// </summary>
        /// <param name="varFromDirectory">源文件夹路径</param>
        /// <param name="varToDirectory">目标文件夹路径</param>
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

        #region 检查文件,如果文件不存在则创建

        /// <summary>
        ///     检查文件,如果文件不存在则创建
        /// </summary>
        /// <param name="FilePath">路径,包括文件名</param>
        public static void ExistsFile(string FilePath)
        {
            //if(!File.Exists(FilePath))    
            //File.Create(FilePath);    
            //以上写法会报错,详细解释请看下文.........   
            if (!File.Exists(FilePath))
            {
                FileStream fs = File.Create(FilePath);
                fs.Close();
            }
        }

        #endregion

        #region 删除指定文件夹对应其他文件夹里的文件

        /// <summary>
        ///     删除指定文件夹对应其他文件夹里的文件
        /// </summary>
        /// <param name="varFromDirectory">指定文件夹路径</param>
        /// <param name="varToDirectory">对应其他文件夹路径</param>
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

        #region 从文件的绝对路径中获取文件名( 包含扩展名 )

        /// <summary>
        ///     从文件的绝对路径中获取文件名( 包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string GetFileName(string filePath)
        {
            //获取文件的名称
            var fi = new FileInfo(filePath);
            return fi.Name;
        }

        #endregion

        /// <summary>
        ///     复制文件参考方法,页面中引用
        /// </summary>
        /// <param name="cDir">新路径</param>
        /// <param name="TempId">模板引擎替换编号</param>
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

        #region 创建一个目录

        /// <summary>
        ///     创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        #endregion

        #region 创建一个文件

        /// <summary>
        ///     创建一个文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    var file = new FileInfo(filePath);

                    //创建文件
                    FileStream fs = file.Create();

                    //关闭文件流
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
        ///     创建一个文件,并将字节流写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    var file = new FileInfo(filePath);

                    //创建文件
                    FileStream fs = file.Create();

                    //写入二进制流
                    fs.Write(buffer, 0, buffer.Length);

                    //关闭文件流
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

        #region 获取文本文件的行数

        /// <summary>
        ///     获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static int GetLineCount(string filePath)
        {
            //将文本文件的各行读到一个字符串数组中
            string[] rows = File.ReadAllLines(filePath);

            //返回行数
            return rows.Length;
        }

        #endregion

        #region 获取一个文件的长度

        /// <summary>
        ///     获取一个文件的长度,单位为Byte
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static int GetFileSize(string filePath)
        {
            //创建一个文件对象
            var fi = new FileInfo(filePath);

            //获取文件的大小
            return (int)fi.Length;
        }

        #endregion

        #region 获取指定目录中的子目录列表

        /// <summary>
        ///     获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">
        ///     模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        ///     范例："Log*.xml"表示搜索所有以Log开头的Xml文件。
        /// </param>
        /// <param name="isSearchChild">是否搜索子目录</param>
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

        #region 向文本文件的尾部追加内容

        /// <summary>
        ///     向文本文件的尾部追加内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">写入的内容</param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }

        #endregion

        #region 将现有文件的内容复制到新文件中

        /// <summary>
        ///     将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }

        #endregion

        #region 将文件移动到指定目录

        /// <summary>
        ///     将文件移动到指定目录
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //获取源文件的名称
            string sourceFileName = GetFileName(sourceFilePath);

            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //将文件移动到指定目录
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }

        #endregion

        #region 清空指定目录

        /// <summary>
        ///     清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }

                //删除目录中所有的子目录
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }

        #endregion

        #region 清空文件内容

        /// <summary>
        ///     清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            //删除文件
            File.Delete(filePath);

            //重新创建该文件
            CreateFile(filePath);
        }

        #endregion

        #region 删除指定目录

        /// <summary>
        ///     删除指定目录及其所有子目录
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        #endregion

        #region 目录

        /// <summary>
        ///     读根目录
        /// </summary>
        public static string GetRootPath()
        {
            return strRootFolder;
        }

        /// <summary>
        ///     写根目录
        /// </summary>
        public static void SetRootPath(string path)
        {
            strRootFolder = path;
        }

        /// <summary>
        ///     读取目录列表
        /// </summary>
        public static List<FileItem> GetDirectoryItems()
        {
            return GetDirectoryItems(strRootFolder);
        }

        /// <summary>
        ///     读取目录列表
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

        #region 文件

        /// <summary>
        ///     读取文件列表
        /// </summary>
        public static List<FileItem> GetFileItems()
        {
            return GetFileItems(strRootFolder);
        }

        /// <summary>
        ///     读取文件列表
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
        ///     创建文件
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
        ///     读取文件
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
        ///     读取文件信息
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
        ///     写入一个新文件，在文件中写入内容，然后关闭文件。如果目标文件已存在，则改写该文件。
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

        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )

        /// <summary>
        ///     从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称
            var fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }

        #endregion

        #region 从文件的绝对路径中获取扩展名

        /// <summary>
        ///     从文件的绝对路径中获取扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string GetExtension(string filePath)
        {
            //获取文件的名称
            var fi = new FileInfo(filePath);
            return fi.Extension;
        }

        #endregion

        #region 文件夹

        /// <summary>
        ///     创建文件夹
        /// </summary>
        public static void CreateFolder(string name, string parentName)
        {
            var di = new DirectoryInfo(parentName);
            di.CreateSubdirectory(name);
        }

        /// <summary>
        ///     移动文件夹
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

        #region 检测文件

        /// <summary>
        ///     判断是否为安全文件名
        /// </summary>
        /// <param name="str">文件名</param>
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
        ///     判断是否为不安全文件名
        /// </summary>
        /// <param name="str">文件名、文件夹名</param>
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
        ///     判断是否为可编辑文件
        /// </summary>
        /// <param name="str">文件名、文件夹名</param>
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

        #region 在当前目录下创建目录

        /****************************************
         * 函数名称：FolderCreate
         * 功能说明：在当前目录下创建目录
         * 参    数：OrignFolder:当前目录,NewFloder:新目录
         * 调用示列：
         *           string OrignFolder = Server.MapPath("test/");    
         *           string NewFloder = "new";
         *           MaxSu.Framework.Common.FileOperate.FolderCreate(OrignFolder, NewFloder); 
        *****************************************/

        /// <summary>
        ///     在当前目录下创建目录
        /// </summary>
        /// <param name="OrignFolder">当前目录</param>
        /// <param name="NewFloder">新目录</param>
        public static void FolderCreate(string OrignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(OrignFolder);
            Directory.CreateDirectory(NewFloder);
        }

        /// <summary>
        ///     创建文件夹
        /// </summary>
        /// <param name="Path"></param>
        public static void FolderCreate(string Path)
        {
            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        #endregion

        #region 递归删除文件夹目录及文件

        /****************************************
         * 函数名称：DeleteFolder
         * 功能说明：递归删除文件夹目录及文件
         * 参    数：dir:文件夹路径
         * 调用示列：
         *           string dir = Server.MapPath("test/");  
         *           MaxSu.Framework.Common.FileOperate.DeleteFolder(dir);       
        *****************************************/

        /// <summary>
        ///     递归删除文件夹目录及文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件                        
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir, true); //删除已空文件夹                 
            }
        }

        #endregion

        #region 将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。

        /****************************************
         * 函数名称：CopyDir
         * 功能说明：将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
         * 参    数：srcPath:原始路径,aimPath:目标文件夹
         * 调用示列：
         *           string srcPath = Server.MapPath("test/");  
         *           string aimPath = Server.MapPath("test1/");
         *           MaxSu.Framework.Common.FileOperate.CopyDir(srcPath,aimPath);   
        *****************************************/

        /// <summary>
        ///     指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath">原始路径</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                        //否则直接Copy文件
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

        #region 获取指定文件夹下所有子目录及文件(树形)

        /****************************************
         * 函数名称：GetFoldAll(string Path)
         * 功能说明：获取指定文件夹下所有子目录及文件(树形)
         * 参    数：Path:详细路径
         * 调用示列：
         *           string strDirlist = Server.MapPath("templates");       
         *           this.Literal1.Text = MaxSu.Framework.Common.FileOperate.GetFoldAll(strDirlist);  
        *****************************************/

        /// <summary>
        ///     获取指定文件夹下所有子目录及文件
        /// </summary>
        /// <param name="Path">详细路径</param>
        public static string GetFoldAll(string Path)
        {
            string str = "";
            var thisOne = new DirectoryInfo(Path);
            str = ListTreeShowAsHtml(thisOne, 0, str);
            return str;
        }

        /// <summary>
        ///     获取指定文件夹下所有子目录及文件函数
        /// </summary>
        /// <param name="theDir">指定目录</param>
        /// <param name="nLevel">默认起始值,调用时,一般为0</param>
        /// <param name="Rn">用于迭加的传入值,一般为空</param>
        /// <returns></returns>
        public static string ListTreeShowAsHtml(DirectoryInfo theDir, int nLevel, string Rn) //递归目录 文件
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories(); //获得目录
            foreach (DirectoryInfo dirinfo in subDirectories)
            {
                if (nLevel == 0)
                {
                    Rn += "├";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "│&nbsp;";
                    }
                    Rn += _s + "├";
                }
                Rn += "<b>" + dirinfo.Name + "</b><br />";
                FileInfo[] fileInfo = dirinfo.GetFiles(); //目录下的文件
                foreach (FileInfo fInfo in fileInfo)
                {
                    if (nLevel == 0)
                    {
                        Rn += "│&nbsp;├";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "│&nbsp;";
                        }
                        Rn += _f + "│&nbsp;├";
                    }
                    Rn += fInfo.Name + " <br />";
                }
                Rn = ListTreeShowAsHtml(dirinfo, nLevel + 1, Rn);
            }
            return Rn;
        }


        /****************************************
     * 函数名称：GetFoldAll(string Path)
     * 功能说明：获取指定文件夹下所有子目录及文件(下拉框形)
     * 参    数：Path:详细路径
     * 调用示列：
     *            string strDirlist = Server.MapPath("templates");      
     *            this.Literal2.Text = MaxSu.Framework.Common.FileOperate.GetFoldAll(strDirlist,"tpl","");
    *****************************************/

        /// <summary>
        ///     获取指定文件夹下所有子目录及文件(下拉框形)
        /// </summary>
        /// <param name="Path">详细路径</param>
        /// <param name="DropName">下拉列表名称</param>
        /// <param name="tplPath">默认选择模板名称</param>
        public static string GetFoldAllAsHtml(string Path, string DropName, string tplPath)
        {
            string strDrop = "<select name=\"" + DropName + "\" id=\"" + DropName +
                             "\"><option value=\"\">--请选择详细模板--</option>";
            string str = "";
            var thisOne = new DirectoryInfo(Path);
            str = ListTreeShowAsHtml(thisOne, 0, str, tplPath);
            return strDrop + str + "</select>";
        }

        /// <summary>
        ///     获取指定文件夹下所有子目录及文件函数
        /// </summary>
        /// <param name="theDir">指定目录</param>
        /// <param name="nLevel">默认起始值,调用时,一般为0</param>
        /// <param name="Rn">用于迭加的传入值,一般为空</param>
        /// <param name="tplPath">默认选择模板名称</param>
        /// <returns></returns>
        public static string ListTreeShowAsHtml(DirectoryInfo theDir, int nLevel, string Rn, string tplPath) //递归目录 文件
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories(); //获得目录

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
                    Rn += "┣";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "│&nbsp;";
                    }
                    Rn += _s + "┣";
                }
                Rn += "" + dirinfo.Name + "</option>";


                FileInfo[] fileInfo = dirinfo.GetFiles(); //目录下的文件
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
                        Rn += "│&nbsp;├";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "│&nbsp;";
                        }
                        Rn += _f + "│&nbsp;├";
                    }
                    Rn += fInfo.Name + "</option>";
                }
                Rn = ListTreeShowAsHtml(dirinfo, nLevel + 1, Rn, tplPath);
            }
            return Rn;
        }

        #endregion

        #region 获取文件夹大小

        /****************************************
         * 函数名称：GetDirectoryLength(string dirPath)
         * 功能说明：获取文件夹大小
         * 参    数：dirPath:文件夹详细路径
         * 调用示列：
         *           string Path = Server.MapPath("templates"); 
         *           Response.Write(MaxSu.Framework.Common.FileOperate.GetDirectoryLength(Path));       
        *****************************************/

        /// <summary>
        ///     获取文件夹大小
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
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

        #region 获取指定文件详细属性

        /****************************************
         * 函数名称：GetFileAttibe(string filePath)
         * 功能说明：获取指定文件详细属性
         * 参    数：filePath:文件详细路径
         * 调用示列：
         *           string file = Server.MapPath("robots.txt");  
         *            Response.Write(MaxSu.Framework.Common.FileOperate.GetFileAttibe(file));         
        *****************************************/

        /// <summary>
        ///     获取指定文件详细属性
        /// </summary>
        /// <param name="filePath">文件详细路径</param>
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