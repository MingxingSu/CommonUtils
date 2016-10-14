using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MaxSu.Framework.Common.Web
{
    /// <summary>
    ///     JavaScript 操作类
    /// </summary>
    public class JavaScriptHelper
    {
        /// <summary>
        ///     弹出信息,并跳转指定页面。
        /// </summary>
        public static void AlertAndRedirect(string message, string toURL)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toURL));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        ///     弹出信息,并返回历史页面
        /// </summary>
        public static void AlertAndGoHistory(string message, int value)
        {
            string js = @"<Script language='JavaScript'>alert('{0}');history.go({1});</Script>";
            HttpContext.Current.Response.Write(string.Format(js, message, value));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        ///     直接跳转到指定的页面
        /// </summary>
        public static void Redirect(string toUrl)
        {
            string js = @"<script language=javascript>window.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, toUrl));
        }

        /// <summary>
        ///     弹出信息 并指定到父窗口
        /// </summary>
        public static void AlertAndParentUrl(string message, string toURL)
        {
            string js = "<script language=javascript>alert('{0}');window.top.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toURL));
        }

        /// <summary>
        ///     返回到父窗口
        /// </summary>
        public static void ParentRedirect(string toUrl)
        {
            string js = "<script language=javascript>window.top.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, toUrl));
        }

        /// <summary>
        ///     弹出信息
        /// </summary>
        public static void Alert(string message)
        {
            string js = "<script language=javascript>alert('{0}');</script>";
            HttpContext.Current.Response.Write(string.Format(js, message));
        }

        /// <summary>
        ///     注册脚本块
        /// </summary>
        public static void RegisterScriptBlock(Page page, string scriptString)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "scriptblock",
                                                    "<script type='text/javascript'>" + scriptString + "</script>");
        }

        /// <summary>
        ///     输出自定义脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseScript(Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message",
                                                    "<script language='javascript' defer>" + script + "</script>");
        }

        /// <summary>
        ///     页面跳转（跳出框架）
        /// </summary>
        /// <param name="url"></param>
        public static void JavaScriptExitIfream(string url)
        {
            string js = @"<Script language='JavaScript'>
                    parent.window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            HttpContext.Current.Response.Write(js);
        }

        #region 回到历史页面

        /// <summary>
        ///     回到历史页面
        /// </summary>
        /// <param name="value">-1/1</param>
        public static void GoHistory(int value)
        {
            #region

            string js = @"<Script language='JavaScript'>
                    history.go({0});  
                  </Script>";
            HttpContext.Current.Response.Write(string.Format(js, value));

            #endregion
        }

        #endregion

        #region 关闭当前窗口

        /// <summary>
        ///     关闭当前窗口
        /// </summary>
        public static void CloseWindow()
        {
            #region

            string js = @"<Script language='JavaScript'>
                    parent.opener=null;window.close();  
                  </Script>";
            HttpContext.Current.Response.Write(js);
            HttpContext.Current.Response.End();

            #endregion
        }

        #endregion

        #region 刷新父窗口

        /// <summary>
        ///     刷新父窗口
        /// </summary>
        public static void RefreshParent(string url)
        {
            #region

            string js = @"<script>try{top.location=""" + url + @"""}catch(e){location=""" + url + @"""}</script>";
            HttpContext.Current.Response.Write(js);

            #endregion
        }

        #endregion

        #region 刷新打开窗口

        /// <summary>
        ///     刷新打开窗口
        /// </summary>
        public static void RefreshOpener()
        {
            #region

            string js = @"<Script language='JavaScript'>
                    opener.location.reload();
                  </Script>";
            HttpContext.Current.Response.Write(js);

            #endregion
        }

        #endregion

        #region 打开指定大小位置的模式对话框

        /// <summary>
        ///     打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">连接地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param>
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            #region

            string features = "dialogWidth:" + width.ToString() + "px"
                              + ";dialogHeight:" + height.ToString() + "px"
                              + ";dialogLeft:" + left.ToString() + "px"
                              + ";dialogTop:" + top.ToString() + "px"
                              + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features);

            #endregion
        }

        #endregion

        #region 打开模式对话框

        /// <summary>
        ///     打开模式对话框
        /// </summary>
        /// <param name="webFormUrl">链接地址</param>
        /// <param name="features"></param>
        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            HttpContext.Current.Response.Write(js);
        }

        /// <summary>
        ///     打开模式对话框
        /// </summary>
        /// <param name="webFormUrl"></param>
        /// <param name="features"></param>
        /// <returns></returns>
        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            #region

            string js = @"<script language=javascript>							
							showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;

            #endregion
        }

        #endregion

        #region 打开指定大小的新窗体

        /// <summary>
        ///     打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        /// <param name="top">头位置</param>
        /// <param name="left">左位置</param>
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {
            #region

            string js = @"<Script language='JavaScript'>window.open('" + url + @"','','height=" + heigth + ",width=" +
                        width + ",top=" + top + ",left=" + left +
                        ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>";

            HttpContext.Current.Response.Write(js);

            #endregion
        }

        #endregion

        #region 显示消息提示对话框

        /// <summary>
        ///     显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void Show(Page page, string msg)
        {
            // page.RegisterStartupScript("message", "<script language='javascript' defer>alert('" + msg.ToString() + "');</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message",
                                                    "<script language='javascript' defer>alert('" + msg + "');</script>");
        }

        #endregion

        #region 控件点击 消息确认提示框

        /// <summary>
        ///     控件点击 消息确认提示框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void ShowConfirm(WebControl Control, string msg)
        {
            //Control.Attributes.Add("onClick","if (!window.confirm('"+msg+"')){return false;}");
            Control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }

        #endregion

        #region 显示消息提示对话框，并进行页面跳转

        /// <summary>
        ///     显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirect(Page page, string msg, string url)
        {
            var Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("location.href='{0}'", url);
            Builder.Append("</script>");
            //page.RegisterStartupScript("message", Builder.ToString());
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());
        }


        public static void ShowAndRedirect(Page page, string msg, string url, bool top)
        {
            var Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            if (top)
            {
                Builder.AppendFormat("top.location.href='{0}'", url);
            }
            else
            {
                Builder.AppendFormat("location.href='{0}'", url);
            }
            Builder.Append("</script>");
            // page.RegisterStartupScript("message", Builder.ToString());
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        public static string GenerateJSConstantsFromEnum(string strNameSpace, string strEnumName)
        {

            string strResult;
            int i;

            try
            {

                strResult = " // Constants for Enum " + strNameSpace + "." + strEnumName + '\n';
                strResult = strResult + "var i = 0;" + '\n';
                strResult = strResult + "var " + strEnumName + " = new Object();" + '\n';

                for (i = 0; i <= Enum.GetNames(Type.GetType(strNameSpace + "." + strEnumName)).Length - 1; i++)
                {

                    strResult = strResult + '\n' + strEnumName + "." + Enum.GetNames(Type.GetType(strNameSpace + "." + strEnumName))[i] + " = i++;";
                    //Type.GetType(strEnumName))(i)

                }

                return strResult + '\n' + '\n';
            }

            catch
            {
                throw;
            }

        }

    }
}