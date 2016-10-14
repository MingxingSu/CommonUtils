using Microsoft.Win32;

namespace MaxSu.Framework.Common
{
	/// <summary>
	/// 注册表相关的工具类
	/// </summary>
	public class RegisterHelper
    {
        #region public static 读取注册表
        ///<summary>
        ///读取注册表
        ///</summary>
        ///<param name="ParaPath">注册表路径</param>
        ///<param name="ParaName">键值</param>
        ///<returns>读出的字符串</returns>
		public static string ReadFromReg(string ParaPath, string ParaName)
		{
			RegistryKey regVersion;
			string strVersion = "";
			
			//得到注册表路径
            regVersion = Registry.LocalMachine.OpenSubKey(ParaPath, false);

			//读出内容
			if(regVersion!=null)
			{
				strVersion = regVersion.GetValue(ParaName, "").ToString();
				regVersion.Close();
			}
			else
			{
				strVersion = "";
			}

			return strVersion;
		}
		#endregion

        #region public static 写入注册表
        ///<summary>
        ///写入注册表
        ///</summary>
        ///<param name="ParaPath">注册表路径</param>
        ///<param name="ParaName">键值</param>
        ///<param name="ParaValue">写入的内容</param>
        public static void WriteReg(string ParaPath,string ParaName,string ParaValue)
		{
			RegistryKey regVersion;

			//读取注册表
            regVersion = Registry.LocalMachine.OpenSubKey(ParaPath, true);

			//在键值内写入数据
			if(regVersion==null)
			{
                //建立键值
                regVersion = Registry.LocalMachine.CreateSubKey(ParaPath);
				regVersion.SetValue(ParaName, ParaValue);
				regVersion.Close();
			}
			else
			{
                //存在键值，直接写入
				regVersion.SetValue(ParaName, ParaValue);
				regVersion.Close();
			}
		}
		#endregion

        #region public static 删除注册表
        ///<summary>
        ///删除注册表
        ///</summary>
        ///<param name="ParaPath">注册表路径</param>
        ///<param name="ParaName">键值</param>
        public static void DeleteReg(string ParaPath,string ParaName)
		{
			RegistryKey regVersion;

            //读取注册表
            regVersion = Registry.LocalMachine.OpenSubKey(ParaPath, true);

			//在键值内写入数据
			if(regVersion!=null)
			{
			    regVersion.DeleteValue(ParaName);
				regVersion.Close(); 
			}
		}
		#endregion
	}
}
