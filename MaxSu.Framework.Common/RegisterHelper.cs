using Microsoft.Win32;

namespace MaxSu.Framework.Common
{
	/// <summary>
	/// ע�����صĹ�����
	/// </summary>
	public class RegisterHelper
    {
        #region public static ��ȡע���
        ///<summary>
        ///��ȡע���
        ///</summary>
        ///<param name="ParaPath">ע���·��</param>
        ///<param name="ParaName">��ֵ</param>
        ///<returns>�������ַ���</returns>
		public static string ReadFromReg(string ParaPath, string ParaName)
		{
			RegistryKey regVersion;
			string strVersion = "";
			
			//�õ�ע���·��
            regVersion = Registry.LocalMachine.OpenSubKey(ParaPath, false);

			//��������
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

        #region public static д��ע���
        ///<summary>
        ///д��ע���
        ///</summary>
        ///<param name="ParaPath">ע���·��</param>
        ///<param name="ParaName">��ֵ</param>
        ///<param name="ParaValue">д�������</param>
        public static void WriteReg(string ParaPath,string ParaName,string ParaValue)
		{
			RegistryKey regVersion;

			//��ȡע���
            regVersion = Registry.LocalMachine.OpenSubKey(ParaPath, true);

			//�ڼ�ֵ��д������
			if(regVersion==null)
			{
                //������ֵ
                regVersion = Registry.LocalMachine.CreateSubKey(ParaPath);
				regVersion.SetValue(ParaName, ParaValue);
				regVersion.Close();
			}
			else
			{
                //���ڼ�ֵ��ֱ��д��
				regVersion.SetValue(ParaName, ParaValue);
				regVersion.Close();
			}
		}
		#endregion

        #region public static ɾ��ע���
        ///<summary>
        ///ɾ��ע���
        ///</summary>
        ///<param name="ParaPath">ע���·��</param>
        ///<param name="ParaName">��ֵ</param>
        public static void DeleteReg(string ParaPath,string ParaName)
		{
			RegistryKey regVersion;

            //��ȡע���
            regVersion = Registry.LocalMachine.OpenSubKey(ParaPath, true);

			//�ڼ�ֵ��д������
			if(regVersion!=null)
			{
			    regVersion.DeleteValue(ParaName);
				regVersion.Close(); 
			}
		}
		#endregion
	}
}
