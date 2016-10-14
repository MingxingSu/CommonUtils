using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using Microsoft.Win32;

namespace MaxSu.Framework.Common.Web
{
    /// <summary>
    ///     ���ļ����ı����ݽ���Multipart��ʽ�ı���
    /// </summary>
    public class MultipartForm
    {
        private readonly string boundary;
        private readonly MemoryStream ms;
        private Encoding encoding;
        private byte[] formData;

        /// <summary>
        ///     ʵ����
        /// </summary>
        public MultipartForm()
        {
            boundary = string.Format("--{0}--", Guid.NewGuid());
            ms = new MemoryStream();
            encoding = Encoding.Default;
        }

        /// <summary>
        ///     ��ȡ�������ֽ�����
        /// </summary>
        public byte[] FormData
        {
            get
            {
                if (formData == null)
                {
                    byte[] buffer = encoding.GetBytes("--" + boundary + "--\r\n");
                    ms.Write(buffer, 0, buffer.Length);
                    formData = ms.ToArray();
                }
                return formData;
            }
        }

        /// <summary>
        ///     ��ȡ�˱������ݵ�����
        /// </summary>
        public string ContentType
        {
            get { return string.Format("multipart/form-data; boundary={0}", boundary); }
        }

        /// <summary>
        ///     ��ȡ�����ö��ַ������õı�������
        /// </summary>
        public Encoding StringEncoding
        {
            set { encoding = value; }
            get { return encoding; }
        }

        /// <summary>
        ///     ���һ���ļ�
        /// </summary>
        /// <param name="name">�ļ�������</param>
        /// <param name="filename">�ļ�������·��</param>
        public void AddFlie(string name, string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("������Ӳ����ڵ��ļ���", filename);
            FileStream fs = null;
            byte[] fileData = {};
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                fileData = new byte[fs.Length];
                fs.Read(fileData, 0, fileData.Length);
                AddFlie(name, Path.GetFileName(filename), fileData, fileData.Length);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        ///     ���һ���ļ�
        /// </summary>
        /// <param name="name">�ļ�������</param>
        /// <param name="filename">�ļ���</param>
        /// <param name="fileData">�ļ�����������</param>
        /// <param name="dataLength">���������ݴ�С</param>
        public void AddFlie(string name, string filename, byte[] fileData, int dataLength)
        {
            if (dataLength <= 0 || dataLength > fileData.Length)
            {
                dataLength = fileData.Length;
            }
            var sb = new StringBuilder();
            sb.AppendFormat("--{0}\r\n", boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\n", name, filename);
            sb.AppendFormat("Content-Type: {0}\r\n", GetContentType(filename));
            sb.Append("\r\n");
            byte[] buf = encoding.GetBytes(sb.ToString());
            ms.Write(buf, 0, buf.Length);
            ms.Write(fileData, 0, dataLength);
            byte[] crlf = encoding.GetBytes("\r\n");
            ms.Write(crlf, 0, crlf.Length);
        }

        /// <summary>
        ///     ����ַ���
        /// </summary>
        /// <param name="name">�ı�������</param>
        /// <param name="value">�ı�ֵ</param>
        public void AddString(string name, string value)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("--{0}\r\n", boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n", name);
            sb.Append("\r\n");
            sb.AppendFormat("{0}\r\n", value);
            byte[] buf = encoding.GetBytes(sb.ToString());
            ms.Write(buf, 0, buf.Length);
        }

        /// <summary>
        ///     ��ע����ȡ�ļ�����
        /// </summary>
        /// <param name="filename">������չ�����ļ���</param>
        /// <returns>�磺application/stream</returns>
        private string GetContentType(string filename)
        {
            RegistryKey fileExtKey = null;
            ;
            string contentType = "application/stream";
            try
            {
                fileExtKey = Registry.ClassesRoot.OpenSubKey(Path.GetExtension(filename));
                contentType = fileExtKey.GetValue("Content Type", contentType).ToString();
            }
            finally
            {
                if (fileExtKey != null) fileExtKey.Close();
            }
            return contentType;
        }

        #region ˽�з���

        /// <summary>
        ///     ��ȡͼƬ��־
        /// </summary>
        private string[] GetImgTag(string htmlStr)
        {
            var regObj = new Regex("<img.+?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var strAry = new string[regObj.Matches(htmlStr).Count];
            int i = 0;
            foreach (Match matchItem in regObj.Matches(htmlStr))
            {
                strAry[i] = GetImgUrl(matchItem.Value);
                i++;
            }
            return strAry;
        }

        /// <summary>
        ///     ��ȡͼƬURL��ַ
        /// </summary>
        private string GetImgUrl(string imgTagStr)
        {
            string str = "";
            var regObj = new Regex("http://.+.(?:jpg|gif|bmp|png)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match matchItem in regObj.Matches(imgTagStr))
            {
                str = matchItem.Value;
            }
            return str;
        }

        #endregion

        /// <summary>
        ///     ����ͼƬ������
        /// </summary>
        /// <param name="strHTML">HTML</param>
        /// <param name="path">·��</param>
        /// <param name="nowyymm">����</param>
        /// <param name="nowdd">��</param>
        public string SaveUrlPics(string strHTML, string path)
        {
            string nowym = DateTime.Now.ToString("yyyy-MM"); //��ǰ����
            string nowdd = DateTime.Now.ToString("dd"); //�������
            path = path + nowym + "/" + nowdd;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string[] imgurlAry = GetImgTag(strHTML);
            try
            {
                for (int i = 0; i < imgurlAry.Length; i++)
                {
                    string preStr = DateTime.Now.ToString() + "_";
                    preStr = preStr.Replace("-", "");
                    preStr = preStr.Replace(":", "");
                    preStr = preStr.Replace(" ", "");
                    var wc = new WebClient();
                    wc.DownloadFile(imgurlAry[i],
                                    path + "/" + preStr + imgurlAry[i].Substring(imgurlAry[i].LastIndexOf("/") + 1));
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return strHTML;
        }

        /// <summary>
        ///     �ļ�����
        /// </summary>
        public enum FileExtension
        {
            JPG = 255216,
            GIF = 7173,
            BMP = 6677,
            PNG = 13780,
            RAR = 8297,
            jpg = 255216,
            exe = 7790,
            xml = 6063,
            html = 6033,
            aspx = 239187,
            cs = 117115,
            js = 119105,
            txt = 210187,
            sql = 255254
        }

        /// <summary>
        ///     ͼƬ�����
        /// </summary>
        public static class FileValidation
        {
            #region �ϴ�ͼƬ�����

            /// <summary>
            ///     �Ƿ�����
            /// </summary>
            public static bool IsAllowedExtension(HttpPostedFile oFile, FileExtension[] fileEx)
            {
                int fileLen = oFile.ContentLength;
                var imgArray = new byte[fileLen];
                oFile.InputStream.Read(imgArray, 0, fileLen);
                var ms = new MemoryStream(imgArray);
                var br = new BinaryReader(ms);
                string fileclass = "";
                byte buffer;
                try
                {
                    buffer = br.ReadByte();
                    fileclass = buffer.ToString();
                    buffer = br.ReadByte();
                    fileclass += buffer.ToString();
                }
                catch
                {
                }
                br.Close();
                ms.Close();
                foreach (FileExtension fe in fileEx)
                {
                    if (Int32.Parse(fileclass) == (int)fe) return true;
                }
                return false;
            }

            /// <summary>
            ///     �ϴ�ǰ��ͼƬ�Ƿ�ɿ�
            /// </summary>
            public static bool IsSecureUploadPhoto(HttpPostedFile oFile)
            {
                bool isPhoto = false;
                string fileExtension = Path.GetExtension(oFile.FileName).ToLower();
                string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        isPhoto = true;
                        break;
                    }
                }
                if (!isPhoto)
                {
                    return true;
                }
                FileExtension[] fe = { FileExtension.BMP, FileExtension.GIF, FileExtension.JPG, FileExtension.PNG };

                if (IsAllowedExtension(oFile, fe))
                    return true;
                else
                    return false;
            }

            /// <summary>
            ///     �ϴ����ͼƬ�Ƿ�ȫ
            /// </summary>
            /// <param name="photoFile">�����ַ</param>
            public static bool IsSecureUpfilePhoto(string photoFile)
            {
                bool isPhoto = false;
                string Img = "Yes";
                string fileExtension = Path.GetExtension(photoFile).ToLower();
                string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        isPhoto = true;
                        break;
                    }
                }

                if (!isPhoto)
                {
                    return true;
                }
                var sr = new StreamReader(photoFile, Encoding.Default);
                string strContent = sr.ReadToEnd();
                sr.Close();
                string str =
                    "request|<script|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
                foreach (string s in str.Split('|'))
                {
                    if (strContent.ToLower().IndexOf(s) != -1)
                    {
                        File.Delete(photoFile);
                        Img = "No";
                        break;
                    }
                }
                return (Img == "Yes");
            }

            #endregion
        }

        /// <summary>
        ///     ͼƬ�ϴ���
        /// </summary>
        //----------------����-------------------
        //imageUpload iu = new imageUpload();
        //iu.AddText = "";
        //iu.CopyIamgePath = "";
        //iu.DrawString_x = ;
        //iu.DrawString_y = ;
        //iu.DrawStyle = ;
        //iu.Font = "";
        //iu.FontSize = ;
        //iu.FormFile = File1;
        //iu.IsCreateImg =;
        //iu.IsDraw = ;
        //iu.OutFileName = "";
        //iu.OutThumbFileName = "";
        //iu.SavePath = @"~/image/";
        //iu.SaveType = ;
        //iu.sHeight  = ;
        //iu.sWidth   = ;
        //iu.Upload();
        //--------------------------------------
        public class ImageUpload
        {
            #region ˽�г�Ա

            private string _AddText = "GlobalNatureCrafts"; //����ˮӡ����
            private string _CopyIamgePath = HttpContext.Current.Server.MapPath(".") + "/images/5dm_new.jpg"; //ͼƬˮӡģʽ�µĸ���ͼƬ��ʵ�ʵ�ַ
            private int _DrawString_x = 10; //�����ı��ģ����꣨���Ͻǣ�
            private int _DrawString_y = 10; //�����ı��ģ����꣨���Ͻǣ�
            private int _DrawStyle; //���ü�ˮӡ�ķ�ʽ��������ˮӡģʽ������ͼƬˮӡģʽ,2:����
            private int _Error; //�����ϴ�״̬�� 
            private int _FileSize; //��ȡ�Ѿ��ϴ��ļ��Ĵ�С
            private string _FileType = "jpg;gif;bmp;png"; //��֧�ֵ��ϴ�������"/"���� 
            private string _Font = "����"; //����ˮӡ����
            private int _FontSize = 12; //����ˮӡ�ִ�С
            private HtmlInputFile _FormFile; //�ϴ��ؼ��� 
            private int _Height; //��ȡ�ϴ�ͼƬ�ĸ߶� 
            private string _InFileName = ""; //���Զ������ļ������á� 
            private bool _IsCreateImg = true; //�Ƿ���������ͼ�� 
            private bool _IsDraw; //�����Ƿ��ˮӡ
            private bool _Iss; //�Ƿ�������ͼ����.
            private int _MaxSize = 1024 * 1024; //��󵥸��ϴ��ļ� (Ĭ��)
            private string _OutFileName = ""; //����ļ����� 
            private string _SavePath = HttpContext.Current.Server.MapPath(".") + "\\"; //�����ļ���ʵ��·�� 
            private int _SaveType; //�ϴ��ļ������ͣ�0�����Զ������ļ��� 
            private int _Width; //��ȡ�ϴ�ͼƬ�Ŀ�� 
            private int _sHeight = 120; //������������ͼ�ĸ߶� 
            private int _sWidth = 120; //������������ͼ�Ŀ��

            #endregion

            #region ��������

            /// <summary>
            ///     Error����ֵ
            ///     1��û���ϴ����ļ�
            ///     2�����Ͳ�����
            ///     3����С����
            ///     4��δ֪����
            ///     0���ϴ��ɹ���
            /// </summary>
            public int Error
            {
                get { return _Error; }
            }

            /// <summary>
            ///     ��󵥸��ϴ��ļ�
            /// </summary>
            public int MaxSize
            {
                set { _MaxSize = value; }
            }

            /// <summary>
            ///     ��֧�ֵ��ϴ�������";"����
            /// </summary>
            public string FileType
            {
                set { _FileType = value; }
            }

            /// <summary>
            ///     �����ļ���ʵ��·��
            /// </summary>
            public string SavePath
            {
                set { _SavePath = HttpContext.Current.Server.MapPath(value); }
                get { return _SavePath; }
            }

            /// <summary>
            ///     �ϴ��ļ������ͣ�0�����Զ������ļ���
            /// </summary>
            public int SaveType
            {
                set { _SaveType = value; }
            }

            /// <summary>
            ///     �ϴ��ؼ�
            /// </summary>
            public HtmlInputFile FormFile
            {
                set { _FormFile = value; }
            }

            /// <summary>
            ///     ���Զ������ļ������á�
            /// </summary>
            public string InFileName
            {
                set { _InFileName = value; }
            }

            /// <summary>
            ///     ����ļ���
            /// </summary>
            public string OutFileName
            {
                get { return _OutFileName; }
                set { _OutFileName = value; }
            }

            /// <summary>
            ///     ���������ͼ�ļ���
            /// </summary>
            public string OutThumbFileName { get; set; }

            /// <summary>
            ///     �Ƿ�������ͼ����.
            /// </summary>
            public bool Iss
            {
                get { return _Iss; }
            }

            /// <summary>
            ///     ��ȡ�ϴ�ͼƬ�Ŀ��
            /// </summary>
            public int Width
            {
                get { return _Width; }
            }

            /// <summary>
            ///     ��ȡ�ϴ�ͼƬ�ĸ߶�
            /// </summary>
            public int Height
            {
                get { return _Height; }
            }

            /// <summary>
            ///     ��������ͼ�Ŀ��
            /// </summary>
            public int sWidth
            {
                get { return _sWidth; }
                set { _sWidth = value; }
            }

            /// <summary>
            ///     ��������ͼ�ĸ߶�
            /// </summary>
            public int sHeight
            {
                get { return _sHeight; }
                set { _sHeight = value; }
            }

            /// <summary>
            ///     �Ƿ���������ͼ
            /// </summary>
            public bool IsCreateImg
            {
                get { return _IsCreateImg; }
                set { _IsCreateImg = value; }
            }

            /// <summary>
            ///     �Ƿ��ˮӡ
            /// </summary>
            public bool IsDraw
            {
                get { return _IsDraw; }
                set { _IsDraw = value; }
            }

            /// <summary>
            ///     ���ü�ˮӡ�ķ�ʽ
            ///     0:����ˮӡģʽ
            ///     1:ͼƬˮӡģʽ
            ///     2:����
            /// </summary>
            public int DrawStyle
            {
                get { return _DrawStyle; }
                set { _DrawStyle = value; }
            }

            /// <summary>
            ///     �����ı��ģ����꣨���Ͻǣ�
            /// </summary>
            public int DrawString_x
            {
                get { return _DrawString_x; }
                set { _DrawString_x = value; }
            }

            /// <summary>
            ///     �����ı��ģ����꣨���Ͻǣ�
            /// </summary>
            public int DrawString_y
            {
                get { return _DrawString_y; }
                set { _DrawString_y = value; }
            }

            /// <summary>
            ///     ��������ˮӡ����
            /// </summary>
            public string AddText
            {
                get { return _AddText; }
                set { _AddText = value; }
            }

            /// <summary>
            ///     ��������ˮӡ����
            /// </summary>
            public string Font
            {
                get { return _Font; }
                set { _Font = value; }
            }

            /// <summary>
            ///     ��������ˮӡ�ֵĴ�С
            /// </summary>
            public int FontSize
            {
                get { return _FontSize; }
                set { _FontSize = value; }
            }

            /// <summary>
            ///     �ļ���С
            /// </summary>
            public int FileSize
            {
                get { return _FileSize; }
                set { _FileSize = value; }
            }

            /// <summary>
            ///     ͼƬˮӡģʽ�µĸ���ͼƬ��ʵ�ʵ�ַ
            /// </summary>
            public string CopyIamgePath
            {
                set { _CopyIamgePath = HttpContext.Current.Server.MapPath(value); }
            }

            #endregion

            #region ˽�з���

            /// <summary>
            ///     ��ȡ�ļ��ĺ�׺��
            /// </summary>
            private string GetExt(string path)
            {
                return Path.GetExtension(path);
            }

            /// <summary>
            ///     ��ȡ����ļ����ļ���
            /// </summary>
            private string FileName(string Ext)
            {
                if (_SaveType == 0 || _InFileName.Trim() == "")
                    return DateTime.Now.ToString("yyyyMMddHHmmssfff") + Ext;
                else
                    return _InFileName;
            }

            /// <summary>
            ///     ����ϴ����ļ������ͣ��Ƿ������ϴ���
            /// </summary>
            private bool IsUpload(string Ext)
            {
                Ext = Ext.Replace(".", "");
                bool b = false;
                string[] arrFileType = _FileType.Split(';');
                foreach (string str in arrFileType)
                {
                    if (str.ToLower() == Ext.ToLower())
                    {
                        b = true;
                        break;
                    }
                }
                return b;
            }

            #endregion

            #region �ϴ�ͼƬ

            public void Upload()
            {
                HttpPostedFile hpFile = _FormFile.PostedFile;
                if (hpFile == null || hpFile.FileName.Trim() == "")
                {
                    _Error = 1;
                    return;
                }
                string Ext = GetExt(hpFile.FileName);
                if (!IsUpload(Ext))
                {
                    _Error = 2;
                    return;
                }
                int iLen = hpFile.ContentLength;
                if (iLen > _MaxSize)
                {
                    _Error = 3;
                    return;
                }
                try
                {
                    if (!Directory.Exists(_SavePath)) Directory.CreateDirectory(_SavePath);
                    var bData = new byte[iLen];
                    hpFile.InputStream.Read(bData, 0, iLen);
                    string FName;
                    FName = FileName(Ext);
                    string TempFile = "";
                    if (_IsDraw)
                    {
                        TempFile = FName.Split('.').GetValue(0) + "_temp." + FName.Split('.').GetValue(1);
                    }
                    else
                    {
                        TempFile = FName;
                    }
                    var newFile = new FileStream(_SavePath + TempFile, FileMode.Create);
                    newFile.Write(bData, 0, bData.Length);
                    newFile.Flush();
                    int _FileSizeTemp = hpFile.ContentLength;

                    string ImageFilePath = _SavePath + FName;
                    if (_IsDraw)
                    {
                        if (_DrawStyle == 0)
                        {
                            Image Img1 = Image.FromStream(newFile);
                            Graphics g = Graphics.FromImage(Img1);
                            g.DrawImage(Img1, 100, 100, Img1.Width, Img1.Height);
                            var f = new Font(_Font, _FontSize);
                            Brush b = new SolidBrush(Color.Red);
                            string addtext = _AddText;
                            g.DrawString(addtext, f, b, _DrawString_x, _DrawString_y);
                            g.Dispose();
                            Img1.Save(ImageFilePath);
                            Img1.Dispose();
                        }
                        else
                        {
                            Image image = Image.FromStream(newFile);
                            Image copyImage = Image.FromFile(_CopyIamgePath);
                            Graphics g = Graphics.FromImage(image);
                            g.DrawImage(copyImage,
                                        new Rectangle(image.Width - copyImage.Width - 5, image.Height - copyImage.Height - 5,
                                                      copyImage.Width, copyImage.Height), 0, 0, copyImage.Width,
                                        copyImage.Height, GraphicsUnit.Pixel);
                            g.Dispose();
                            image.Save(ImageFilePath);
                            image.Dispose();
                        }
                    }

                    //��ȡͼƬ�ĸ߶ȺͿ��
                    Image Img = Image.FromStream(newFile);
                    _Width = Img.Width;
                    _Height = Img.Height;

                    //��������ͼ���� 
                    if (_IsCreateImg)
                    {
                        #region ����ͼ��Сֻ���������Χ��������ʵ�ʴ�С

                        float realbili = _Width / (float)_Height;
                        float wishbili = _sWidth / (float)_sHeight;

                        //ʵ��ͼ������ͼ���ߴ�������Կ�Ϊ׼
                        if (realbili > wishbili)
                        {
                            _sHeight = (int)(_sWidth / realbili);
                        }
                            //ʵ��ͼ������ͼ���ߴ���߳����Ը�Ϊ׼
                        else
                        {
                            _sWidth = (int)(_sHeight * realbili);
                        }

                        #endregion

                        OutThumbFileName = FName.Split('.').GetValue(0) + "_s." + FName.Split('.').GetValue(1);
                        string ImgFilePath = _SavePath + OutThumbFileName;

                        Image newImg = Img.GetThumbnailImage(_sWidth, _sHeight, null, IntPtr.Zero);
                        newImg.Save(ImgFilePath);
                        newImg.Dispose();
                        _Iss = true;
                    }

                    if (_IsDraw)
                    {
                        if (File.Exists(_SavePath + FName.Split('.').GetValue(0) + "_temp." + FName.Split('.').GetValue(1)))
                        {
                            newFile.Dispose();
                            File.Delete(_SavePath + FName.Split('.').GetValue(0) + "_temp." + FName.Split('.').GetValue(1));
                        }
                    }
                    newFile.Close();
                    newFile.Dispose();
                    _OutFileName = FName;
                    _FileSize = _FileSizeTemp;
                    _Error = 0;
                    return;
                }
                catch (Exception ex)
                {
                    _Error = 4;
                    return;
                }
            }

            #endregion
        }
    }
}