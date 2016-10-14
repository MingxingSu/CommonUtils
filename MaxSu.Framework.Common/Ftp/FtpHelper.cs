using System;
using System.IO;
using System.Text;

namespace MaxSu.Framework.Common.Ftp
{
    public class FtpHelper
    {
        #region ����

        private FtpClient _ftpClient;

        /// <summary>
        ///     ȫ��FTP���ʱ���
        /// </summary>
        public FtpClient FtpClient
        {
            get { return _ftpClient; }
            set { _ftpClient = value; }
        }

        /// <summary>
        ///     Ftp������
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        ///     Ftp�û�
        /// </summary>
        public string User { get; set; }

        /// <summary>
        ///     Ftp����
        /// </summary>
        public string Pass { get; set; }

        /// <summary>
        ///     Ftp����
        /// </summary>
        public string FolderZJ { get; set; }

        /// <summary>
        ///     Ftp����
        /// </summary>
        public string FolderWX { get; set; }

        #endregion

        /// <summary>
        ///     �õ��ļ��б�
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
        ///     �����ļ�
        /// </summary>
        /// <param name="ftpFolder">ftpĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
        /// <param name="localFolder">����Ŀ¼</param>
        /// <param name="localFileName">�����ļ���</param>
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
        ///     �޸��ļ�
        /// </summary>
        /// <param name="ftpFolder">����Ŀ¼</param>
        /// <param name="ftpFileName">�����ļ���temp</param>
        /// <param name="localFolder">����Ŀ¼</param>
        /// <param name="localFileName">�����ļ���</param>
        public bool AddMSCFile(string ftpFolder, string ftpFileName, string localFolder, string localFileName,
                               string BscInfo)
        {
            string sLine = "";
            string sResult = "";
            string path = "���Ӧ�ó������ڵ�������·��";
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
        ///     ɾ���ļ�
        /// </summary>
        /// <param name="ftpFolder">ftpĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
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
        ///     �ϴ��ļ�
        /// </summary>
        /// <param name="ftpFolder">ftpĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
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
        ///     �����ļ�
        /// </summary>
        /// <param name="ftpFolder">ftpĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
        /// <param name="localFolder">����Ŀ¼</param>
        /// <param name="localFileName">�����ļ���</param>
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
        ///     �õ�FTP���ļ���Ϣ
        /// </summary>
        /// <param name="ftpFolder">FTPĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
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
        ///     ����FTP�������Ƿ�ɵ�½
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
        ///     �õ�FTP���ļ���Ϣ
        /// </summary>
        /// <param name="ftpFolder">FTPĿ¼</param>
        /// <param name="ftpFileName">ftp�ļ���</param>
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
        ///     �õ��ļ��б�
        /// </summary>
        /// <param name="ftpFolder">FTPĿ¼</param>
        /// <returns>FTPͨ�����</returns>
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
        ///     �õ�FTP�������
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