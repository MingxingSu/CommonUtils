//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2010 , Jirisoft , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MaxSu.Framework.Common.ResourceManager
{
    /// <summary>
    ///     ResourceManager
    ///     资源管理器
    ///     修改纪录
    ///     2007.05.16 版本：1.0 JiRiGaLa	重新调整代码的规范化。
    ///     版本：1.0
    ///     <author>
    ///         <name>JiRiGaLa</name>
    ///         <date>2007.05.16</date>
    ///     </author>
    /// </summary>
    public class ResourceManager
    {
        private static volatile ResourceManager instance;
        private static readonly object locker = new Object();

        private string FolderPath = string.Empty;
        public SortedList<String, Resources> LanguageResources = new SortedList<String, Resources>();

        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ResourceManager();
                        }
                    }
                }
                return instance;
            }
        }

        public void Serialize(Resources resources, string filePath)
        {
            ResourcesSerializer.Serialize(filePath, resources);
        }

        public void Init(string filePath)
        {
            FolderPath = filePath;
            var directoryInfo = new DirectoryInfo(filePath);
            LanguageResources.Clear();
            if (!directoryInfo.Exists)
            {
                return;
            }
            FileInfo[] FileInfo = directoryInfo.GetFiles();
            for (int i = 0; i < FileInfo.Length; i++)
            {
                Resources resources = ResourcesSerializer.DeSerialize(FileInfo[i].FullName);
                resources.createIndex();
                LanguageResources.Add(resources.language, resources);
            }
        }

        public Hashtable GetLanguages()
        {
            var hashtable = new Hashtable();
            IEnumerator<KeyValuePair<String, Resources>> iEnumerator = LanguageResources.GetEnumerator();
            while (iEnumerator.MoveNext())
            {
                hashtable.Add(iEnumerator.Current.Key, iEnumerator.Current.Value.displayName);
            }
            return hashtable;
        }

        public Hashtable GetLanguages(string path)
        {
            var hashtable = new Hashtable();
            var directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                return hashtable;
            }
            FileInfo[] fileInfo = directoryInfo.GetFiles();
            for (int i = 0; i < fileInfo.Length; i++)
            {
                Resources resources = ResourcesSerializer.DeSerialize(fileInfo[i].FullName);
                hashtable.Add(resources.language, resources.displayName);
            }
            return hashtable;
        }

        public Resources GetResources(string filePath)
        {
            var resources = new Resources();
            if (File.Exists(filePath))
            {
                resources = ResourcesSerializer.DeSerialize(filePath);
                resources.createIndex();
            }
            return resources;
        }

        public string Get(string language, string key)
        {
            if (!LanguageResources.ContainsKey(language))
            {
                return string.Empty;
            }
            return LanguageResources[language].Get(key);
        }
    }
}