using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace MaxSu.Framework.Common.Web
{
    public class Fetch
    {

        /// <summary>
        /// </summary>
        public enum SortType
        {
            /// <summary>
            /// </summary>
            Photo = 1,

            /// <summary>
            /// </summary>
            Article = 5,

            /// <summary>
            /// </summary>
            Diary = 7,

            /// <summary>
            /// </summary>
            Pic = 2,

            /// <summary>
            /// </summary>
            Music = 6,

            /// <summary>
            /// </summary>
            AddressList = 4,

            /// <summary>
            /// </summary>
            Favorite = 3,
        }

        #region 根据给出的相对地址获取网站绝对地址

        /// <summary>
        ///     根据给出的相对地址获取网站绝对地址
        /// </summary>
        /// <param name="localPath">相对地址</param>
        /// <returns>绝对地址</returns>
        public static string GetWebPath(string localPath)
        {
            string path = HttpContext.Current.Request.ApplicationPath;
            string thisPath;
            string thisLocalPath;
            //如果不是根目录就加上"/" 根目录自己会加"/"
            if (path != "/")
            {
                thisPath = path + "/";
            }
            else
            {
                thisPath = path;
            }
            if (localPath.StartsWith("~/"))
            {
                thisLocalPath = localPath.Substring(2);
            }
            else
            {
                return localPath;
            }
            return thisPath + thisLocalPath;
        }

        #endregion

        #region 获取网站绝对地址

        /// <summary>
        ///     获取网站绝对地址
        /// </summary>
        /// <returns></returns>
        public static string GetWebPath()
        {
            string path = HttpContext.Current.Request.ApplicationPath;
            string thisPath;
            //如果不是根目录就加上"/" 根目录自己会加"/"
            if (path != "/")
            {
                thisPath = path + "/";
            }
            else
            {
                thisPath = path;
            }
            return thisPath;
        }

        #endregion

        #region 根据相对路径或绝对路径获取绝对路径

        /// <summary>
        ///     根据相对路径或绝对路径获取绝对路径
        /// </summary>
        /// <param name="localPath">相对路径或绝对路径</param>
        /// <returns>绝对路径</returns>
        public static string GetFilePath(string localPath)
        {
            if (Regex.IsMatch(localPath, @"([A-Za-z]):\\([\S]*)"))
            {
                return localPath;
            }
            else
            {
                return HttpContext.Current.Server.MapPath(localPath);
            }
        }

        #endregion


        public static string CurrentUrl
        {
            get { return HttpContext.Current.Request.Url.ToString(); }
        }

        public static string ServerDomain
        {
            get
            {
                string urlHost = HttpContext.Current.Request.Url.Host.ToLower();
                string[] urlHostArray = urlHost.Split(new[] {'.'});
                if ((urlHostArray.Length < 3) || RegexDao.IsIp(urlHost))
                {
                    return urlHost;
                }
                string urlHost2 = urlHost.Remove(0, urlHost.IndexOf(".") + 1);
                if ((urlHost2.StartsWith("com.") || urlHost2.StartsWith("net.")) ||
                    (urlHost2.StartsWith("org.") || urlHost2.StartsWith("gov.")))
                {
                    return urlHost;
                }
                return urlHost2;
            }
        }


        /// <summary>
        ///     获得用户IP
        /// </summary>
        public static string GetUserIp()
        {
            string ip;
            string[] temp;
            bool isErr = false;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            else
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"];
            if (ip.Length > 15)
                isErr = true;
            else
            {
                temp = ip.Split('.');
                if (temp.Length == 4)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3) isErr = true;
                    }
                }
                else
                    isErr = true;
            }

            if (isErr)
                return "1.1.1.1";
            else
                return ip;
        }


        public static string Get(string name)
        {
            string text1 = HttpContext.Current.Request.QueryString[name];
            return ((text1 == null) ? "" : text1.Trim());
        }

        public static string Post(string name)
        {
            string text1 = HttpContext.Current.Request.Form[name];
            return ((text1 == null) ? "" : text1.Trim());
        }

        public static int GetQueryId(string name)
        {
            int id = 0;
            int.TryParse(Get(name), out id);
            return id;
        }

        public static int[] GetIds(string name)
        {
            string ids = Post(name);
            var result = new List<int>();
            int id = 0;
            string[] array = ids.Split(',');
            foreach (string a in array)
                if (int.TryParse(a.Trim(), out id))
                    result.Add(id);

            return result.ToArray();
        }

        public static int[] GetQueryIds(string name)
        {
            string ids = Get(name);
            var result = new List<int>();
            int id = 0;
            string[] array = ids.Split(',');
            foreach (string a in array)
                if (int.TryParse(a.Trim(), out id))
                    result.Add(id);

            return result.ToArray();
        }


        #region 类内部调用

        /// <summary>
        ///     HttpContext Current
        /// </summary>
        public static HttpContext Current
        {
            get { return HttpContext.Current; }
        }

        /// <summary>
        ///     HttpContext Current  HttpRequest Request   get { return Current.Request;
        /// </summary>
        public static HttpRequest Request
        {
            get { return Current.Request; }
        }

        /// <summary>
        ///     HttpContext Current  HttpRequest Request   get { return Current.Request; HttpResponse Response  return Current.Response;
        /// </summary>
        public static HttpResponse Response
        {
            get { return Current.Response; }
        }

        #endregion


        /// <summary>
        ///
        /// </summary>
        public static string GetUserLanguage()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper(); ;
        }

        /// <summary>
        ///
        /// </summary>
        public static string GetCurrentCulture()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
        }
    }
}