using System.Web;

namespace MaxSu.Framework.Common.Web
{
    /// <summary>
    ///     Session 操作类
    ///     1、GetSession(string name)根据session名获取session对象
    ///     2、SetSession(string name, object val)设置session
    /// </summary>
    public class SessionLibrary
    {
        /// <summary>
        ///     根据session名获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];
        }

        /// <summary>
        ///     设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="val">session 值</param>
        public static void SetSession(string name, object val)
        {
            HttpContext.Current.Session.Remove(name);
            HttpContext.Current.Session.Add(name, val);
        }
    }
}