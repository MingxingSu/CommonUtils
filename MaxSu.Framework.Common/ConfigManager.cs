using System;
using System.Windows.Forms;
using System.Xml;

namespace MaxSu.Framework.Common
{
    //==============================================
    //        FileName: ConfigManager
    //        Description: ��̬����ҵ���࣬���ڶ�C#��ASP.NET�е�WinForm & WebForm ��Ŀ���������ļ�
    //             app.config��web.config��[appSettings]��[connectionStrings]�ڵ�����������޸ġ�ɾ���Ͷ�ȡ��صĲ�����

    //==============================================

    /// <summary>
    ///     ConfigManager Ӧ�ó��������ļ�������
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        ///     ��[appSettings]�ڵ�����Keyֵ��ȡ��Valueֵ�������ַ���
        /// </summary>
        /// <param name="configurationFile">Ҫ�����������ļ�����,ö�ٳ���</param>
        /// <param name="key">Ҫ��ȡ��Keyֵ</param>
        /// <returns>����Valueֵ���ַ���</returns>
        public static string ReadValueByKey(ConfigurationFile configurationFile, string key)
        {
            string value = string.Empty;
            string filename = string.Empty;
            if (configurationFile.ToString() == ConfigurationFile.AppConfig.ToString())
            {
                filename = Application.ExecutablePath + ".config";
            }
            else
            {
                filename = AppDomain.CurrentDomain.BaseDirectory + "web.config";
            }

            var doc = new XmlDocument();
            doc.Load(filename); //���������ļ�

            XmlNode node = doc.SelectSingleNode("//appSettings"); //�õ�[appSettings]�ڵ�

            ////�õ�[appSettings]�ڵ��й���Key���ӽڵ�
            var element = (XmlElement) node.SelectSingleNode("//add[@key='" + key + "']");

            if (element != null)
            {
                value = element.GetAttribute("value");
            }

            return value;
        }

        /// <summary>
        ///     ��[connectionStrings]�ڵ�����nameֵ��ȡ��connectionStringֵ�������ַ���
        /// </summary>
        /// <param name="configurationFile">Ҫ�����������ļ�����,ö�ٳ���</param>
        /// <param name="name">Ҫ��ȡ��nameֵ</param>
        /// <returns>����connectionStringֵ���ַ���</returns>
        public static string ReadConnectionStringByName(ConfigurationFile configurationFile, string name)
        {
            string connectionString = string.Empty;
            string filename = string.Empty;
            if (configurationFile.ToString() == ConfigurationFile.AppConfig.ToString())
            {
                filename = Application.ExecutablePath + ".config";
            }
            else
            {
                filename = AppDomain.CurrentDomain.BaseDirectory + "web.config";
            }

            var doc = new XmlDocument();
            doc.Load(filename); //���������ļ�

            XmlNode node = doc.SelectSingleNode("//connectionStrings"); //�õ�[appSettings]�ڵ�

            ////�õ�[connectionString]�ڵ��й���name���ӽڵ�
            var element = (XmlElement) node.SelectSingleNode("//add[@name='" + name + "']");

            if (element != null)
            {
                connectionString = element.GetAttribute("connectionString");
            }

            return connectionString;
        }

        /// <summary>
        ///     ���»�����[appSettings]�ڵ���ӽڵ�ֵ������������ӽڵ�Value,�������������ӽڵ㣬���سɹ���񲼶�ֵ
        /// </summary>
        /// <param name="configurationFile">Ҫ�����������ļ�����,ö�ٳ���</param>
        /// <param name="key">�ӽڵ�Keyֵ</param>
        /// <param name="value">�ӽڵ�valueֵ</param>
        /// <returns>���سɹ���񲼶�ֵ</returns>
        public static bool UpdateOrCreateAppSetting(ConfigurationFile configurationFile, string key, string value)
        {
            bool isSuccess = false;
            string filename = string.Empty;
            if (configurationFile.ToString() == ConfigurationFile.AppConfig.ToString())
            {
                filename = Application.ExecutablePath + ".config";
            }
            else
            {
                filename = AppDomain.CurrentDomain.BaseDirectory + "web.config";
            }

            var doc = new XmlDocument();
            doc.Load(filename); //���������ļ�

            XmlNode node = doc.SelectSingleNode("//appSettings"); //�õ�[appSettings]�ڵ�

            try
            {
                ////�õ�[appSettings]�ڵ��й���Key���ӽڵ�
                var element = (XmlElement) node.SelectSingleNode("//add[@key='" + key + "']");

                if (element != null)
                {
                    //����������ӽڵ�Value
                    element.SetAttribute("value", value);
                }
                else
                {
                    //�������������ӽڵ�
                    XmlElement subElement = doc.CreateElement("add");
                    subElement.SetAttribute("key", key);
                    subElement.SetAttribute("value", value);
                    node.AppendChild(subElement);
                }

                //�����������ļ�(��ʽһ)
                using (var xmlwriter = new XmlTextWriter(filename, null))
                {
                    xmlwriter.Formatting = Formatting.Indented;
                    doc.WriteTo(xmlwriter);
                    xmlwriter.Flush();
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        ///     ���»�����[connectionStrings]�ڵ���ӽڵ�ֵ������������ӽڵ�,�������������ӽڵ㣬���سɹ���񲼶�ֵ
        /// </summary>
        /// <param name="configurationFile">Ҫ�����������ļ�����,ö�ٳ���</param>
        /// <param name="name">�ӽڵ�nameֵ</param>
        /// <param name="connectionString">�ӽڵ�connectionStringֵ</param>
        /// <param name="providerName">�ӽڵ�providerNameֵ</param>
        /// <returns>���سɹ���񲼶�ֵ</returns>
        public static bool UpdateOrCreateConnectionString(ConfigurationFile configurationFile, string name,
                                                          string connectionString, string providerName)
        {
            bool isSuccess = false;
            string filename = string.Empty;
            if (configurationFile.ToString() == ConfigurationFile.AppConfig.ToString())
            {
                filename = Application.ExecutablePath + ".config";
            }
            else
            {
                filename = AppDomain.CurrentDomain.BaseDirectory + "web.config";
            }

            var doc = new XmlDocument();
            doc.Load(filename); //���������ļ�

            XmlNode node = doc.SelectSingleNode("//connectionStrings"); //�õ�[connectionStrings]�ڵ�

            try
            {
                ////�õ�[connectionStrings]�ڵ��й���Name���ӽڵ�
                var element = (XmlElement) node.SelectSingleNode("//add[@name='" + name + "']");

                if (element != null)
                {
                    //����������ӽڵ�
                    element.SetAttribute("connectionString", connectionString);
                    element.SetAttribute("providerName", providerName);
                }
                else
                {
                    //�������������ӽڵ�
                    XmlElement subElement = doc.CreateElement("add");
                    subElement.SetAttribute("name", name);
                    subElement.SetAttribute("connectionString", connectionString);
                    subElement.SetAttribute("providerName", providerName);
                    node.AppendChild(subElement);
                }

                //�����������ļ�(��ʽ��)
                doc.Save(filename);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        ///     ɾ��[appSettings]�ڵ��а���Keyֵ���ӽڵ㣬���سɹ���񲼶�ֵ
        /// </summary>
        /// <param name="configurationFile">Ҫ�����������ļ�����,ö�ٳ���</param>
        /// <param name="key">Ҫɾ�����ӽڵ�Keyֵ</param>
        /// <returns>���سɹ���񲼶�ֵ</returns>
        public static bool DeleteByKey(ConfigurationFile configurationFile, string key)
        {
            bool isSuccess = false;
            string filename = string.Empty;
            if (configurationFile.ToString() == ConfigurationFile.AppConfig.ToString())
            {
                filename = Application.ExecutablePath + ".config";
            }
            else
            {
                filename = AppDomain.CurrentDomain.BaseDirectory + "web.config";
            }

            var doc = new XmlDocument();
            doc.Load(filename); //���������ļ�

            XmlNode node = doc.SelectSingleNode("//appSettings"); //�õ�[appSettings]�ڵ�

            ////�õ�[appSettings]�ڵ��й���Key���ӽڵ�
            var element = (XmlElement) node.SelectSingleNode("//add[@key='" + key + "']");

            if (element != null)
            {
                //������ɾ���ӽڵ�
                element.ParentNode.RemoveChild(element);
            }
            else
            {
                //������
            }

            try
            {
                //�����������ļ�(��ʽһ)
                using (var xmlwriter = new XmlTextWriter(filename, null))
                {
                    xmlwriter.Formatting = Formatting.Indented;
                    doc.WriteTo(xmlwriter);
                    xmlwriter.Flush();
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        ///     ɾ��[connectionStrings]�ڵ��а���nameֵ���ӽڵ㣬���سɹ���񲼶�ֵ
        /// </summary>
        /// <param name="configurationFile">Ҫ�����������ļ�����,ö�ٳ���</param>
        /// <param name="name">Ҫɾ�����ӽڵ�nameֵ</param>
        /// <returns>���سɹ���񲼶�ֵ</returns>
        public static bool DeleteByName(ConfigurationFile configurationFile, string name)
        {
            bool isSuccess = false;
            string filename = string.Empty;
            if (configurationFile.ToString() == ConfigurationFile.AppConfig.ToString())
            {
                filename = Application.ExecutablePath + ".config";
            }
            else
            {
                filename = AppDomain.CurrentDomain.BaseDirectory + "web.config";
            }

            var doc = new XmlDocument();
            doc.Load(filename); //���������ļ�

            XmlNode node = doc.SelectSingleNode("//connectionStrings"); //�õ�[connectionStrings]�ڵ�

            ////�õ�[connectionStrings]�ڵ��й���Name���ӽڵ�
            var element = (XmlElement) node.SelectSingleNode("//add[@name='" + name + "']");

            if (element != null)
            {
                //������ɾ���ӽڵ�
                node.RemoveChild(element);
            }
            else
            {
                //������
            }

            try
            {
                //�����������ļ�(��ʽ��)
                doc.Save(filename);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}