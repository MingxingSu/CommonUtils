//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2010 , Jirisoft , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;

namespace MaxSu.Framework.Common.ResourceManager
{
    /// <summary>
    ///     ResourceManagerWrapper
    ///     ��Դ������
    ///     �޸ļ�¼
    ///     2007.05.16 �汾��1.0 JiRiGaLa	���µ�������Ĺ淶����
    ///     �汾��1.0
    ///     <author>
    ///         <name>JiRiGaLa</name>
    ///         <date>2007.05.16</date>
    ///     </author>
    /// </summary>
    public class ResourceManagerWrapper
    {
        private static volatile ResourceManagerWrapper instance;
        private static readonly object locker = new Object();
        private static string CurrentLanguage = "en-us";

        private ResourceManager ResourceManager;

        public static ResourceManagerWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ResourceManagerWrapper();
                        }
                    }
                }
                return instance;
            }
        }

        public void LoadResources(string path)
        {
            ResourceManager = ResourceManager.Instance;
            ResourceManager.Init(path);
        }

        public string Get(string key)
        {
            return ResourceManager.Get(CurrentLanguage, key);
        }

        public string Get(string lanauage, string key)
        {
            return ResourceManager.Get(lanauage, key);
        }

        public Hashtable GetLanguages()
        {
            return ResourceManager.GetLanguages();
        }

        public Hashtable GetLanguages(string path)
        {
            return ResourceManager.GetLanguages(path);
        }

        public void Serialize(string path, string language, string key, string value)
        {
            Resources Resources = GetResources(path, language);
            Resources.Set(key, value);
            string filePath = path + "\\" + language + ".xml";
            ResourceManager.Serialize(Resources, filePath);
        }

        public Resources GetResources(string path, string language)
        {
            string filePath = path + "\\" + language + ".xml";
            return ResourceManager.GetResources(filePath);
        }

        public Resources GetResources(string language)
        {
            return ResourceManager.LanguageResources[language];
        }
    }
}