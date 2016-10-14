    using MaxSu.Framework.Common;
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml;

namespace MaxSu.Framework.Common.Configuration
{
    internal sealed class ApplicationConfiguration
    {
        private const string rootFolder = "Azur";
        private const string configFileNotFound = "The configuration file %%1 couldn't be found.";

        private const string errConfigItemDoesNotExist =
            "The configuration item %%1 doesn't exist in the configuration file %%2.";

        private const string azFrameworkAppConfigFolder = "AppConfigurationFiles";
        private string mstrConfigFilename;
        private XmlDocument mxmlDoc;

        public string GetConfigValue(string strConfigItemName, string strDefaultValue = "")
        {
            string innerXml;
            try
            {
                XmlNode node = this.mxmlDoc.DocumentElement.SelectSingleNode(strConfigItemName);
                if (node == null)
                {
                    if (strDefaultValue == null)
                    {
                        throw new ApplicationException(
                            StringLibrary.ReplaceParameters(
                                "The configuration item %%1 doesn't exist in the configuration file %%2.",
                                ArrayLibrary.CreateArray(new object[] { strConfigItemName, this.mstrConfigFilename })));
                    }
                    return strDefaultValue;
                }
                innerXml = node.InnerXml;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
                ProjectData.ClearProjectError();
            }
            return innerXml;
        }

        private string GetCurrentAssemblyVersion()
        {
            string str;
            try
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                long major = version.Major;
                long minor = version.Minor;
                long revision = version.Revision;
                str = Conversions.ToString(major) + "." + Conversions.ToString(minor) + "." + Conversions.ToString(revision);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
                ProjectData.ClearProjectError();
            }
            return str;
        }

        private string GetDefaultConfigurationFileName()
        {
            string str;
            try
            {
                AssemblyName name = Assembly.GetCallingAssembly().GetName();
                string str2 = name.Name;
                long major = name.Version.Major;
                long minor = name.Version.Minor;
                long revision = name.Version.Revision;
                int index = str2.IndexOf(".");
                index = str2.IndexOf(".", (int)(index + 1));
                if (index > 0)
                {
                    str2 = str2.Substring(0, index);
                }
                str = str2 + "." + Conversions.ToString(major) + "." + Conversions.ToString(minor) + "." +
                      Conversions.ToString(revision);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
                ProjectData.ClearProjectError();
            }
            return str;
        }

        public void Init(string strConfigFileName)
        {
            try
            {
                this.mstrConfigFilename = this.AzurConfigurationFilesPath + @"\" + strConfigFileName + ".Config.xml";
                if (!File.Exists(this.mstrConfigFilename))
                {
                    throw new ApplicationException(
                        StringLibrary.ReplaceParameters("The configuration file %%1 couldn't be found.",
                                                        ArrayLibrary.CreateArray(new object[] { this.mstrConfigFilename })));
                }
                this.mxmlDoc = new XmlDocument();
                this.mxmlDoc.Load(this.mstrConfigFilename);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
                ProjectData.ClearProjectError();
            }
        }

        public void SetConfigValue(string strConfigItemName, string strValue)
        {
            try
            {
                XmlNode node = this.mxmlDoc.DocumentElement.SelectSingleNode(strConfigItemName);
                if (node == null)
                {
                    node = this.mxmlDoc.DocumentElement.AppendChild(this.mxmlDoc.CreateElement(strConfigItemName));
                }
                node.InnerXml = strValue;
                this.mxmlDoc.Save(this.mstrConfigFilename);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw exception;
            }
        }

        public string AzurApplicationsRootPath
        {
            get { return (Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Azur"); }
        }

        public string AzurConfigurationFilesPath
        {
            get { return (this.AzurApplicationsRootPath + @"\AppConfigurationFiles"); }
        }

        public bool this[string strConfigItemName]
        {
            get { return (this.mxmlDoc.DocumentElement.SelectSingleNode(strConfigItemName) != null); }
        }
    }
}

