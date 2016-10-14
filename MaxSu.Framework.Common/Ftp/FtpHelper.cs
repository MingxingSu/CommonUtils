using System;
using System.IO;
using System.Text;

namespace MaxSu.Framework.Common.Ftp
{
    public class FtpHelper
    {
        #region 属性

        private FtpClient _ftpClient;

        /// <summary>
        ///     全局FTP访问变量
        /// </summary>
        public FtpClient FtpClient
        {
            get { return _ftpClient; }
            set { _ftpClient = value; }
        }

        /// <summary>
        ///     Ftp服务器
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        ///     Ftp用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        ///     Ftp密码
        /// </summary>
        public string Pass { get; set; }

        /// <summary>
        ///     Ftp密码
        /// </summary>
        public string FolderZJ { get; set; }

        /// <summary>
        ///     Ftp密码
        /// </summary>
        public string FolderWX { get; set; }

        #endregion

        /// <summary>
        ///     得到文件列表
        /// </summary>
        /// <returns></returns>
        public string[] GetList(string strPath)
        {
            if (_ftpClient == null) _ftpClient = getFtpClient();
            _ftpClient.Connect();
            _ftpClient.ChDir(strPath);
            return _ftpClient.Dir("*");
        }

        /// <summary>
        ///     下载文件
        /// </summary>
        /// <param name="ftpFolder">ftp目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        /// <param name="localFolder">本地目录</param>
        /// <param name="localFileName">本地文件名</param>
        public bool GetFile(string ftpFolder, string ftpFileName, string localFolder, string localFileName)
        {
            try
            {
                if (_ftpClient == null) _ftpClient = getFtpClient();
                if (!_ftpClient.Connected)
                {
                    _ftpClient.Connect();
                    _ftpClient.ChDir(ftpFolder);
                }
                _ftpClient.Download(ftpFileName, localFolder, localFileName);

                return true;
            }
            catch
            {
                try
                {
                    _ftpClient.DisConnect();
                    _ftpClient = null;
                }
                catch
                {
                    _ftpClient = null;
                }
                return false;
            }
        }

        /// <summary>
        ///     修改文件
        /// </summary>
        /// <param name="ftpFolder">本地目录</param>
        /// <param name="ftpFileName">本地文件名temp</param>
        /// <param name="localFolder">本地目录</param>
        /// <param name="localFileName">本地文件名</param>
        public bool AddMSCFile(string ftpFolder, string ftpFileName, string localFolder, string localFileName,
                               string BscInfo)
        {
            string sLine = "";
            string sResult = "";
            string path = "获得应用程序所在的完整的路径";
            path = path.Substring(0, path.LastIndexOf("\\"));
            try
            {
                var fsFile = new FileStream(ftpFolder + "\\" + ftpFileName, FileMode.Open);
                var fsFileWrite = new FileStream(localFolder + "\\" + localFileName, FileMode.Create);
                var sr = new StreamReader(fsFile);
                var sw = new StreamWriter(fsFileWrite);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                while (sr.Peek() > -1)
                {
                    sLine = sr.ReadToEnd();
                }
                string[] arStr = sLine.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < arStr.Length - 1; i++)
                {
                    sResult += BscInfo + "," + arStr[i].Trim() + "\n";
                }
                sr.Close();
                byte[] connect = new UTF8Encoding(true).GetBytes(sResult);
                fsFileWrite.Write(connect, 0, connect.Length);
                fsFileWrite.Flush();
                sw.Close();
                fsFile.Close();
                fsFileWrite.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        ///     删除文件
        /// </summary>
        /// <param name="ftpFolder">ftp目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        public bool DelFile(string ftpFolder, string ftpFileName)
        {
            try
            {
                if (_ftpClient == null) _ftpClient = getFtpClient();
                if (!_ftpClient.Connected)
                {
                    _ftpClient.Connect();
                    _ftpClient.ChDir(ftpFolder);
                }
                _ftpClient.Delete(ftpFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="ftpFolder">ftp目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        public bool PutFile(string ftpFolder, string ftpFileName)
        {
            try
            {
                if (_ftpClient == null) _ftpClient = getFtpClient();
                if (!_ftpClient.Connected)
                {
                    _ftpClient.Connect();
                    _ftpClient.ChDir(ftpFolder);
                }
                _ftpClient.Put(ftpFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     下载文件
        /// </summary>
        /// <param name="ftpFolder">ftp目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        /// <param name="localFolder">本地目录</param>
        /// <param name="localFileName">本地文件名</param>
        public bool GetFileNoBinary(string ftpFolder, string ftpFileName, string localFolder, string localFileName)
        {
            try
            {
                if (_ftpClient == null) _ftpClient = getFtpClient();
                if (!_ftpClient.Connected)
                {
                    _ftpClient.Connect();
                    _ftpClient.ChDir(ftpFolder);
                }
                _ftpClient.GetNoBinary(ftpFileName, localFolder, localFileName);
                return true;
            }
            catch
            {
                try
                {
                    _ftpClient.DisConnect();
                    _ftpClient = null;
                }
                catch
                {
                    _ftpClient = null;
                }
                return false;
            }
        }

        /// <summary>
        ///     得到FTP上文件信息
        /// </summary>
        /// <param name="ftpFolder">FTP目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        public string GetFileInfo(string ftpFolder, string ftpFileName)
        {
            string strResult = "";
            try
            {
                if (_ftpClient == null) _ftpClient = getFtpClient();
                if (_ftpClient.Connected) _ftpClient.DisConnect();
                _ftpClient.Connect();
                _ftpClient.ChDir(ftpFolder);
                strResult = _ftpClient.GetFileInfo(ftpFileName);
                return strResult;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        ///     测试FTP服务器是否可登陆
        /// </summary>
        public bool CanConnect()
        {
            if (_ftpClient == null) _ftpClient = getFtpClient();
            try
            {
                _ftpClient.Connect();
                _ftpClient.DisConnect();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     得到FTP上文件信息
        /// </summary>
        /// <param name="ftpFolder">FTP目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        public string GetFileInfoConnected(string ftpFolder, string ftpFileName)
        {
            string strResult = "";
            try
            {
                if (_ftpClient == null) _ftpClient = getFtpClient();
                if (!_ftpClient.Connected)
                {
                    _ftpClient.Connect();
                    _ftpClient.ChDir(ftpFolder);
                }
                strResult = _ftpClient.GetFileInfo(ftpFileName);
                return strResult;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        ///     得到文件列表
        /// </summary>
        /// <param name="ftpFolder">FTP目录</param>
        /// <returns>FTP通配符号</returns>
        public string[] GetFileList(string ftpFolder, string strMask)
        {
            string[] strResult;
            try
            {
                if (_ftpClient == null) _ftpClient = getFtpClient();
                if (!_ftpClient.Connected)
                {
                    _ftpClient.Connect();
                    _ftpClient.ChDir(ftpFolder);
                }
                strResult = _ftpClient.Dir(strMask);
                return strResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     得到FTP传输对象
        /// </summary>
        public FtpClient getFtpClient()
        {
            var ft = new FtpClient();
            ft.RemoteHost = Server;
            ft.RemoteUser = User;
            ft.RemotePass = Pass;
            return ft;
        }
    }
}