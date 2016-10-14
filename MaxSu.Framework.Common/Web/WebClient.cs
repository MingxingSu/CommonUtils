using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MaxSu.Framework.Common.Web
{
    public class WebClient
    {
        private static CookieContainer cc;
        private readonly WebHeaderCollection requestHeaders;
        private int bufferSize = 15240;
        private Encoding encoding = Encoding.Default;
        private WebProxy proxy;
        private string respHtml = "";
        private WebHeaderCollection responseHeaders;

        static WebClient()
        {
            LoadCookiesFromDisk();
        }

        /// <summary>
        ///     ����WebClient��ʵ��
        /// </summary>
        public WebClient()
        {
            requestHeaders = new WebHeaderCollection();
            responseHeaders = new WebHeaderCollection();
        }

        /// <summary>
        ///     ���÷��ͺͽ��յ����ݻ����С
        /// </summary>
        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        /// <summary>
        ///     ��ȡ��Ӧͷ����
        /// </summary>
        public WebHeaderCollection ResponseHeaders
        {
            get { return responseHeaders; }
        }

        /// <summary>
        ///     ��ȡ����ͷ����
        /// </summary>
        public WebHeaderCollection RequestHeaders
        {
            get { return requestHeaders; }
        }

        /// <summary>
        ///     ��ȡ�����ô���
        /// </summary>
        public WebProxy Proxy
        {
            get { return proxy; }
            set { proxy = value; }
        }

        /// <summary>
        ///     ��ȡ��������������Ӧ���ı����뷽ʽ
        /// </summary>
        public Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        /// <summary>
        ///     ��ȡ��������Ӧ��html����
        /// </summary>
        public string RespHtml
        {
            get { return respHtml; }
            set { respHtml = value; }
        }

        /// <summary>
        ///     ��ȡ�����������������Cookie����
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return cc; }
            set { cc = value; }
        }

        public event EventHandler<UploadEventArgs> UploadProgressChanged;
        public event EventHandler<DownloadEventArgs> DownloadProgressChanged;

        /// <summary>
        ///     ��ȡ��ҳԴ����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns></returns>
        public string GetHtml(string url)
        {
            HttpWebRequest request = CreateRequest(url, "GET");
            respHtml = encoding.GetString(GetData(request));
            return respHtml;
        }

        /// <summary>
        ///     �����ļ�
        /// </summary>
        /// <param name="url">�ļ�URL��ַ</param>
        /// <param name="filename">�ļ���������·��</param>
        public void DownloadFile(string url, string filename)
        {
            FileStream fs = null;
            try
            {
                HttpWebRequest request = CreateRequest(url, "GET");
                byte[] data = GetData(request);
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                fs.Write(data, 0, data.Length);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        ///     ��ָ��URL��������
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns></returns>
        public byte[] GetData(string url)
        {
            HttpWebRequest request = CreateRequest(url, "GET");
            return GetData(request);
        }

        /// <summary>
        ///     ��ָ��URL�����ı�����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <param name="postData">urlencode������ı�����</param>
        /// <returns></returns>
        public string Post(string url, string postData)
        {
            byte[] data = encoding.GetBytes(postData);
            return Post(url, data);
        }

        /// <summary>
        ///     ��ָ��URL�����ֽ�����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <param name="postData">���͵��ֽ�����</param>
        /// <returns></returns>
        public string Post(string url, byte[] postData)
        {
            HttpWebRequest request = CreateRequest(url, "POST");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.KeepAlive = true;
            PostData(request, postData);
            respHtml = encoding.GetString(GetData(request));
            return respHtml;
        }

        /// <summary>
        ///     ��ָ����ַ����mulitpart���������
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <param name="mulitpartForm">mulitpart form data</param>
        /// <returns></returns>
        public string Post(string url, MultipartForm mulitpartForm)
        {
            HttpWebRequest request = CreateRequest(url, "POST");
            request.ContentType = mulitpartForm.ContentType;
            request.ContentLength = mulitpartForm.FormData.Length;
            request.KeepAlive = true;
            PostData(request, mulitpartForm.FormData);
            respHtml = encoding.GetString(GetData(request));
            return respHtml;
        }

        /// <summary>
        ///     ��ȡ���󷵻ص�����
        /// </summary>
        /// <param name="request">�������</param>
        /// <returns></returns>
        private byte[] GetData(HttpWebRequest request)
        {
            var response = (HttpWebResponse) request.GetResponse();
            Stream stream = response.GetResponseStream();
            responseHeaders = response.Headers;
            //SaveCookiesToDisk();

            var args = new DownloadEventArgs();
            if (responseHeaders[HttpResponseHeader.ContentLength] != null)
                args.TotalBytes = Convert.ToInt32(responseHeaders[HttpResponseHeader.ContentLength]);

            var ms = new MemoryStream();
            int count = 0;
            var buf = new byte[bufferSize];
            while ((count = stream.Read(buf, 0, buf.Length)) > 0)
            {
                ms.Write(buf, 0, count);
                if (DownloadProgressChanged != null)
                {
                    args.BytesReceived += count;
                    args.ReceivedData = new byte[count];
                    Array.Copy(buf, args.ReceivedData, count);
                    DownloadProgressChanged(this, args);
                }
            }
            stream.Close();
            //��ѹ    
            if (ResponseHeaders[HttpResponseHeader.ContentEncoding] != null)
            {
                var msTemp = new MemoryStream();
                count = 0;
                buf = new byte[100];
                switch (ResponseHeaders[HttpResponseHeader.ContentEncoding].ToLower())
                {
                    case "gzip":
                        var gzip = new GZipStream(ms, CompressionMode.Decompress);
                        while ((count = gzip.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }
                        return msTemp.ToArray();
                    case "deflate":
                        var deflate = new DeflateStream(ms, CompressionMode.Decompress);
                        while ((count = deflate.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }
                        return msTemp.ToArray();
                    default:
                        break;
                }
            }
            return ms.ToArray();
        }

        /// <summary>
        ///     ������������
        /// </summary>
        /// <param name="request">�������</param>
        /// <param name="postData">�����͵��ֽ�����</param>
        private void PostData(HttpWebRequest request, byte[] postData)
        {
            int offset = 0;
            int sendBufferSize = bufferSize;
            int remainBytes = 0;
            Stream stream = request.GetRequestStream();
            var args = new UploadEventArgs();
            args.TotalBytes = postData.Length;
            while ((remainBytes = postData.Length - offset) > 0)
            {
                if (sendBufferSize > remainBytes) sendBufferSize = remainBytes;
                stream.Write(postData, offset, sendBufferSize);
                offset += sendBufferSize;
                if (UploadProgressChanged != null)
                {
                    args.BytesSent = offset;
                    UploadProgressChanged(this, args);
                }
            }
            stream.Close();
        }

        /// <summary>
        ///     ����HTTP����
        /// </summary>
        /// <param name="url">URL��ַ</param>
        /// <returns></returns>
        private HttpWebRequest CreateRequest(string url, string method)
        {
            var uri = new Uri(url);

            if (uri.Scheme == "https")
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;

            // Set a default policy level for the "http:" and "https" schemes.    
            var policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
            HttpWebRequest.DefaultCachePolicy = policy;

            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.AllowAutoRedirect = false;
            request.AllowWriteStreamBuffering = false;
            request.Method = method;
            if (proxy != null)
                request.Proxy = proxy;
            request.CookieContainer = cc;
            foreach (string key in requestHeaders.Keys)
            {
                request.Headers.Add(key, requestHeaders[key]);
            }
            requestHeaders.Clear();
            return request;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
                                           SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        ///     ��Cookie���浽����
        /// </summary>
        private static void SaveCookiesToDisk()
        {
            string cookieFile = Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
            FileStream fs = null;
            try
            {
                fs = new FileStream(cookieFile, FileMode.Create);
                var formater = new BinaryFormatter();
                formater.Serialize(fs, cc);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        ///     �Ӵ��̼���Cookie
        /// </summary>
        private static void LoadCookiesFromDisk()
        {
            cc = new CookieContainer();
            string cookieFile = Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
            if (!File.Exists(cookieFile))
                return;
            FileStream fs = null;
            try
            {
                fs = new FileStream(cookieFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                var formater = new BinaryFormatter();
                cc = (CookieContainer) formater.Deserialize(fs);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }
    }
}