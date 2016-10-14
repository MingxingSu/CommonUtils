using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace MaxSu.Framework.Common
{
    //if (this.fload.HasFile)
    //{
    //    string upFileName = HttpContext.Current.Server.MapPath("~/savefile") + "\\" + this.fload.PostedFile.FileName;
    //    string saveName   = DateTime.Now.ToString("yyyyMMddHHmmssffff");
    //    string playFile   = Server.MapPath(VideoConvert.savefile + saveName);
    //    string imgFile    = Server.MapPath(VideoConvert.savefile + saveName);

    //    VideoConvert pm = new VideoConvert();
    //    string m_strExtension = VideoConvert.GetExtension(this.fload.PostedFile.FileName).ToLower();
    //    if (m_strExtension == "flv")
    //    {
    //        System.IO.File.Copy(upFileName, playFile + ".flv");
    //        pm.CatchImg(upFileName, imgFile);
    //    }
    //    string Extension = pm.CheckExtension(m_strExtension);
    //    if (Extension == "ffmpeg")
    //    {
    //        pm.ChangeFilePhy(upFileName, playFile, imgFile);
    //    }
    //    else if (Extension == "mencoder")
    //    {
    //        pm.MChangeFilePhy(upFileName, playFile, imgFile);
    //    }
    //}
    public class VideoConvert : Page
    {
        private readonly string[] strArrFfmpeg = new[] {"asf", "avi", "mpg", "3gp", "mov"};
        private readonly string[] strArrMencoder = new[] {"wmv", "rmvb", "rm"};

        #region ����

        public static string ffmpegtool = ConfigurationManager.AppSettings["ffmpeg"];
        public static string mencodertool = ConfigurationManager.AppSettings["mencoder"];
        public static string savefile = ConfigurationManager.AppSettings["savefile"] + "/";
        public static string sizeOfImg = ConfigurationManager.AppSettings["CatchFlvImgSize"];
        public static string widthOfFile = ConfigurationManager.AppSettings["widthSize"];
        public static string heightOfFile = ConfigurationManager.AppSettings["heightSize"];

        #endregion

        #region ��ȡ�ļ�������

        /// <summary>
        ///     ��ȡ�ļ�������
        /// </summary>
        public static string GetFileName(string fileName)
        {
            int i = fileName.LastIndexOf("\\") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }

        #endregion

        #region ��ȡ�ļ���չ��

        /// <summary>
        ///     ��ȡ�ļ���չ��
        /// </summary>
        public static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }

        #endregion

        #region ��ȡ�ļ�����

        /// <summary>
        ///     ��ȡ�ļ�����
        /// </summary>
        public string CheckExtension(string extension)
        {
            string m_strReturn = "";
            foreach (string var in strArrFfmpeg)
            {
                if (var == extension)
                {
                    m_strReturn = "ffmpeg";
                    break;
                }
            }
            if (m_strReturn == "")
            {
                foreach (string var in strArrMencoder)
                {
                    if (var == extension)
                    {
                        m_strReturn = "mencoder";
                        break;
                    }
                }
            }
            return m_strReturn;
        }

        #endregion

        #region ��Ƶ��ʽתΪFlv

        /// <summary>
        ///     ��Ƶ��ʽתΪFlv
        /// </summary>
        /// <param name="vFileName">ԭ��Ƶ�ļ���ַ</param>
        /// <param name="ExportName">���ɺ��Flv�ļ���ַ</param>
        public bool ConvertFlv(string vFileName, string ExportName)
        {
            if ((!File.Exists(ffmpegtool)) || (!File.Exists(HttpContext.Current.Server.MapPath(vFileName))))
            {
                return false;
            }
            vFileName = HttpContext.Current.Server.MapPath(vFileName);
            ExportName = HttpContext.Current.Server.MapPath(ExportName);
            string Command = " -i \"" + vFileName + "\" -y -ab 32 -ar 22050 -b 800000 -s  480*360 \"" + ExportName +
                             "\"";
            //Flv��ʽ     
            var p = new Process();
            p.StartInfo.FileName = ffmpegtool;
            p.StartInfo.Arguments = Command;
            p.StartInfo.WorkingDirectory = HttpContext.Current.Server.MapPath("~/tools/");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.BeginErrorReadLine();
            p.WaitForExit();
            p.Close();
            p.Dispose();
            return true;
        }

        #endregion

        #region ����Flv��Ƶ������ͼ

        /// <summary>
        ///     ����Flv��Ƶ������ͼ
        /// </summary>
        /// <param name="vFileName">��Ƶ�ļ���ַ</param>
        public string CatchImg(string vFileName)
        {
            if ((!File.Exists(ffmpegtool)) || (!File.Exists(HttpContext.Current.Server.MapPath(vFileName)))) return "";
            try
            {
                string flv_img_p = vFileName.Substring(0, vFileName.Length - 4) + ".jpg";
                string Command = " -i " + HttpContext.Current.Server.MapPath(vFileName) + " -y -f image2 -t 0.1 -s " +
                                 sizeOfImg + " " + HttpContext.Current.Server.MapPath(flv_img_p);
                var p = new Process();
                p.StartInfo.FileName = ffmpegtool;
                p.StartInfo.Arguments = Command;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                try
                {
                    p.Start();
                }
                catch
                {
                    return "";
                }
                finally
                {
                    p.Close();
                    p.Dispose();
                }
                Thread.Sleep(4000);

                //ע��:ͼƬ��ȡ�ɹ���,�������ڴ滺��д��������Ҫʱ��ϳ�,�����3,4����������;
                if (File.Exists(HttpContext.Current.Server.MapPath(flv_img_p)))
                {
                    return flv_img_p;
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region ����FFMpeg����Ƶ����(����·��)

        /// <summary>
        ///     ת���ļ���������ָ���ļ�����
        /// </summary>
        /// <param name="fileName">�ϴ���Ƶ�ļ���·����ԭ�ļ���</param>
        /// <param name="playFile">ת������ļ���·�������粥���ļ���</param>
        /// <param name="imgFile">����Ƶ�ļ���ץȡ��ͼƬ·��</param>
        /// <returns>�ɹ�:����ͼƬ�����ַ;ʧ��:���ؿ��ַ���</returns>
        public string ChangeFilePhy(string fileName, string playFile, string imgFile)
        {
            string ffmpeg = Server.MapPath(ffmpegtool);
            if ((!File.Exists(ffmpeg)) || (!File.Exists(fileName)))
            {
                return "";
            }
            string flv_file = Path.ChangeExtension(playFile, ".flv");
            string FlvImgSize = sizeOfImg;
            var FilestartInfo = new ProcessStartInfo(ffmpeg);
            FilestartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            FilestartInfo.Arguments = " -i " + fileName + " -ab 56 -ar 22050 -b 500 -r 15 -s " + widthOfFile + "x" +
                                      heightOfFile + " " + flv_file;
            try
            {
                Process.Start(FilestartInfo); //ת��
                CatchImg(fileName, imgFile); //��ͼ
            }
            catch
            {
                return "";
            }
            return "";
        }

        public string CatchImg(string fileName, string imgFile)
        {
            string ffmpeg = Server.MapPath(ffmpegtool);
            string flv_img = imgFile + ".jpg";
            string FlvImgSize = sizeOfImg;
            var ImgstartInfo = new ProcessStartInfo(ffmpeg);
            ImgstartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ImgstartInfo.Arguments = "   -i   " + fileName + "  -y  -f  image2   -ss 2 -vframes 1  -s   " + FlvImgSize +
                                     "   " + flv_img;
            try
            {
                Process.Start(ImgstartInfo);
            }
            catch
            {
                return "";
            }
            if (File.Exists(flv_img))
            {
                return flv_img;
            }
            return "";
        }

        #endregion

        #region ����FFMpeg����Ƶ����(���·��)

        /// <summary>
        ///     ת���ļ���������ָ���ļ�����
        /// </summary>
        /// <param name="fileName">�ϴ���Ƶ�ļ���·����ԭ�ļ���</param>
        /// <param name="playFile">ת������ļ���·�������粥���ļ���</param>
        /// <param name="imgFile">����Ƶ�ļ���ץȡ��ͼƬ·��</param>
        /// <returns>�ɹ�:����ͼƬ�����ַ;ʧ��:���ؿ��ַ���</returns>
        public string ChangeFileVir(string fileName, string playFile, string imgFile)
        {
            string ffmpeg = Server.MapPath(ffmpegtool);
            if ((!File.Exists(ffmpeg)) || (!File.Exists(fileName)))
            {
                return "";
            }
            string flv_img = Path.ChangeExtension(Server.MapPath(imgFile), ".jpg");
            string flv_file = Path.ChangeExtension(Server.MapPath(playFile), ".flv");
            string FlvImgSize = sizeOfImg;

            var ImgstartInfo = new ProcessStartInfo(ffmpeg);
            ImgstartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ImgstartInfo.Arguments = "   -i   " + fileName + "   -y   -f   image2   -t   0.001   -s   " + FlvImgSize +
                                     "   " +
                                     flv_img;

            var FilestartInfo = new ProcessStartInfo(ffmpeg);
            FilestartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            FilestartInfo.Arguments = " -i " + fileName + " -ab 56 -ar 22050 -b 500 -r 15 -s " + widthOfFile + "x" +
                                      heightOfFile + " " + flv_file;
            try
            {
                Process.Start(FilestartInfo);
                Process.Start(ImgstartInfo);
            }
            catch
            {
                return "";
            }

            ///ע��:ͼƬ��ȡ�ɹ���,�������ڴ滺��д��������Ҫʱ��ϳ�,�����3,4����������;   
            ///�����Ҫ��ʱ���ټ��,�ҷ�������ʱ8��,���������8��ͼƬ�Բ�����,��Ϊ��ͼʧ��;    
            if (File.Exists(flv_img))
            {
                return flv_img;
            }
            return "";
        }

        #endregion

        #region ����mencoder����Ƶ������ת��(����·��)

        /// <summary>
        ///     ����mencoder����Ƶ������ת��
        /// </summary>
        public string MChangeFilePhy(string vFileName, string playFile, string imgFile)
        {
            string tool = Server.MapPath(mencodertool);
            if ((!File.Exists(tool)) || (!File.Exists(vFileName)))
            {
                return "";
            }
            string flv_file = Path.ChangeExtension(playFile, ".flv");
            string FlvImgSize = sizeOfImg;
            var FilestartInfo = new ProcessStartInfo(tool);
            FilestartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            FilestartInfo.Arguments = " " + vFileName + " -o " + flv_file +
                                      " -of lavf -lavfopts i_certify_that_my_video_stream_does_not_use_b_frames -oac mp3lame -lameopts abr:br=56 -ovc lavc -lavcopts vcodec=flv:vbitrate=200:mbd=2:mv0:trell:v4mv:cbp:last_pred=1:dia=-1:cmp=0:vb_strategy=1 -vf scale=" +
                                      widthOfFile + ":" + heightOfFile + " -ofps 12 -srate 22050";
            try
            {
                Process.Start(FilestartInfo);
                CatchImg(flv_file, imgFile);
            }
            catch
            {
                return "";
            }
            return "";
        }

        #endregion
    }
}