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
    ///     对文件和文本数据进行Multipart形式的编码
    /// </summary>
    public class MultipartForm
    {
        private readonly string boundary;
        private readonly MemoryStream ms;
        private Encoding encoding;
        private byte[] formData;

        /// <summary>
        ///     实例化
        /// </summary>
        public MultipartForm()
        {
            boundary = string.Format("--{0}--", Guid.NewGuid());
            ms = new MemoryStream();
            encoding = Encoding.Default;
        }

        /// <summary>
        ///     获取编码后的字节数组
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
        ///     获取此编码内容的类型
        /// </summary>
        public string ContentType
        {
            get { return string.Format("multipart/form-data; boundary={0}", boundary); }
        }

        /// <summary>
        ///     获取或设置对字符串采用的编码类型
        /// </summary>
        public Encoding StringEncoding
        {
            set { encoding = value; }
            get { return encoding; }
        }

        /// <summary>
        ///     添加一个文件
        /// </summary>
        /// <param name="name">文件域名称</param>
        /// <param name="filename">文件的完整路径</param>
        public void AddFlie(string name, string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("尝试添加不存在的文件。", filename);
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
        ///     添加一个文件
        /// </summary>
        /// <param name="name">文件域名称</param>
        /// <param name="filename">文件名</param>
        /// <param name="fileData">文件二进制数据</param>
        /// <param name="dataLength">二进制数据大小</param>
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
        ///     添加字符串
        /// </summary>
        /// <param name="name">文本域名称</param>
        /// <param name="value">文本值</param>
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
        ///     从注册表获取文件类型
        /// </summary>
        /// <param name="filename">包含扩展名的文件名</param>
        /// <returns>如：application/stream</returns>
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

        #region 私有方法

        /// <summary>
        ///     获取图片标志
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
        ///     获取图片URL地址
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
        ///     下载图片到本地
        /// </summary>
        /// <param name="strHTML">HTML</param>
        /// <param name="path">路径</param>
        /// <param name="nowyymm">年月</param>
        /// <param name="nowdd">日</param>
        public string SaveUrlPics(string strHTML, string path)
        {
            string nowym = DateTime.Now.ToString("yyyy-MM"); //当前年月
            string nowdd = DateTime.Now.ToString("dd"); //当天号数
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
        ///     文件类型
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
        ///     图片检测类
        /// </summary>
        public static class FileValidation
        {
            #region 上传图片检测类

            /// <summary>
            ///     是否允许
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
            ///     上传前的图片是否可靠
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
            ///     上传后的图片是否安全
            /// </summary>
            /// <param name="photoFile">物理地址</param>
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
        ///     图片上传类
        /// </summary>
        //----------------调用-------------------
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
            #region 私有成员

            private string _AddText = "GlobalNatureCrafts"; //设置水印内容
            private string _CopyIamgePath = HttpContext.Current.Server.MapPath(".") + "/images/5dm_new.jpg"; //图片水印模式下的覆盖图片的实际地址
            private int _DrawString_x = 10; //绘制文本的Ｘ坐标（左上角）
            private int _DrawString_y = 10; //绘制文本的Ｙ坐标（左上角）
            private int _DrawStyle; //设置加水印的方式０：文字水印模式，１：图片水印模式,2:不加
            private int _Error; //返回上传状态。 
            private int _FileSize; //获取已经上传文件的大小
            private string _FileType = "jpg;gif;bmp;png"; //所支持的上传类型用"/"隔开 
            private string _Font = "宋体"; //设置水印字体
            private int _FontSize = 12; //设置水印字大小
            private HtmlInputFile _FormFile; //上传控件。 
            private int _Height; //获取上传图片的高度 
            private string _InFileName = ""; //非自动生成文件名设置。 
            private bool _IsCreateImg = true; //是否生成缩略图。 
            private bool _IsDraw; //设置是否加水印
            private bool _Iss; //是否有缩略图生成.
            private int _MaxSize = 1024 * 1024; //最大单个上传文件 (默认)
            private string _OutFileName = ""; //输出文件名。 
            private string _SavePath = HttpContext.Current.Server.MapPath(".") + "\\"; //保存文件的实际路径 
            private int _SaveType; //上传文件的类型，0代表自动生成文件名 
            private int _Width; //获取上传图片的宽度 
            private int _sHeight = 120; //设置生成缩略图的高度 
            private int _sWidth = 120; //设置生成缩略图的宽度

            #endregion

            #region 公有属性

            /// <summary>
            ///     Error返回值
            ///     1、没有上传的文件
            ///     2、类型不允许
            ///     3、大小超限
            ///     4、未知错误
            ///     0、上传成功。
            /// </summary>
            public int Error
            {
                get { return _Error; }
            }

            /// <summary>
            ///     最大单个上传文件
            /// </summary>
            public int MaxSize
            {
                set { _MaxSize = value; }
            }

            /// <summary>
            ///     所支持的上传类型用";"隔开
            /// </summary>
            public string FileType
            {
                set { _FileType = value; }
            }

            /// <summary>
            ///     保存文件的实际路径
            /// </summary>
            public string SavePath
            {
                set { _SavePath = HttpContext.Current.Server.MapPath(value); }
                get { return _SavePath; }
            }

            /// <summary>
            ///     上传文件的类型，0代表自动生成文件名
            /// </summary>
            public int SaveType
            {
                set { _SaveType = value; }
            }

            /// <summary>
            ///     上传控件
            /// </summary>
            public HtmlInputFile FormFile
            {
                set { _FormFile = value; }
            }

            /// <summary>
            ///     非自动生成文件名设置。
            /// </summary>
            public string InFileName
            {
                set { _InFileName = value; }
            }

            /// <summary>
            ///     输出文件名
            /// </summary>
            public string OutFileName
            {
                get { return _OutFileName; }
                set { _OutFileName = value; }
            }

            /// <summary>
            ///     输出的缩略图文件名
            /// </summary>
            public string OutThumbFileName { get; set; }

            /// <summary>
            ///     是否有缩略图生成.
            /// </summary>
            public bool Iss
            {
                get { return _Iss; }
            }

            /// <summary>
            ///     获取上传图片的宽度
            /// </summary>
            public int Width
            {
                get { return _Width; }
            }

            /// <summary>
            ///     获取上传图片的高度
            /// </summary>
            public int Height
            {
                get { return _Height; }
            }

            /// <summary>
            ///     设置缩略图的宽度
            /// </summary>
            public int sWidth
            {
                get { return _sWidth; }
                set { _sWidth = value; }
            }

            /// <summary>
            ///     设置缩略图的高度
            /// </summary>
            public int sHeight
            {
                get { return _sHeight; }
                set { _sHeight = value; }
            }

            /// <summary>
            ///     是否生成缩略图
            /// </summary>
            public bool IsCreateImg
            {
                get { return _IsCreateImg; }
                set { _IsCreateImg = value; }
            }

            /// <summary>
            ///     是否加水印
            /// </summary>
            public bool IsDraw
            {
                get { return _IsDraw; }
                set { _IsDraw = value; }
            }

            /// <summary>
            ///     设置加水印的方式
            ///     0:文字水印模式
            ///     1:图片水印模式
            ///     2:不加
            /// </summary>
            public int DrawStyle
            {
                get { return _DrawStyle; }
                set { _DrawStyle = value; }
            }

            /// <summary>
            ///     绘制文本的Ｘ坐标（左上角）
            /// </summary>
            public int DrawString_x
            {
                get { return _DrawString_x; }
                set { _DrawString_x = value; }
            }

            /// <summary>
            ///     绘制文本的Ｙ坐标（左上角）
            /// </summary>
            public int DrawString_y
            {
                get { return _DrawString_y; }
                set { _DrawString_y = value; }
            }

            /// <summary>
            ///     设置文字水印内容
            /// </summary>
            public string AddText
            {
                get { return _AddText; }
                set { _AddText = value; }
            }

            /// <summary>
            ///     设置文字水印字体
            /// </summary>
            public string Font
            {
                get { return _Font; }
                set { _Font = value; }
            }

            /// <summary>
            ///     设置文字水印字的大小
            /// </summary>
            public int FontSize
            {
                get { return _FontSize; }
                set { _FontSize = value; }
            }

            /// <summary>
            ///     文件大小
            /// </summary>
            public int FileSize
            {
                get { return _FileSize; }
                set { _FileSize = value; }
            }

            /// <summary>
            ///     图片水印模式下的覆盖图片的实际地址
            /// </summary>
            public string CopyIamgePath
            {
                set { _CopyIamgePath = HttpContext.Current.Server.MapPath(value); }
            }

            #endregion

            #region 私有方法

            /// <summary>
            ///     获取文件的后缀名
            /// </summary>
            private string GetExt(string path)
            {
                return Path.GetExtension(path);
            }

            /// <summary>
            ///     获取输出文件的文件名
            /// </summary>
            private string FileName(string Ext)
            {
                if (_SaveType == 0 || _InFileName.Trim() == "")
                    return DateTime.Now.ToString("yyyyMMddHHmmssfff") + Ext;
                else
                    return _InFileName;
            }

            /// <summary>
            ///     检查上传的文件的类型，是否允许上传。
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

            #region 上传图片

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

                    //获取图片的高度和宽度
                    Image Img = Image.FromStream(newFile);
                    _Width = Img.Width;
                    _Height = Img.Height;

                    //生成缩略图部分 
                    if (_IsCreateImg)
                    {
                        #region 缩略图大小只设置了最大范围，并不是实际大小

                        float realbili = _Width / (float)_Height;
                        float wishbili = _sWidth / (float)_sHeight;

                        //实际图比缩略图最大尺寸更宽矮，以宽为准
                        if (realbili > wishbili)
                        {
                            _sHeight = (int)(_sWidth / realbili);
                        }
                            //实际图比缩略图最大尺寸更高长，以高为准
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