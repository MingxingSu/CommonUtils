using System;

namespace MaxSu.Framework.Common
{
    [Serializable]
    public class FileItem
    {
        #region 私有字段

        private DateTime _CreationDate;
        private int _FileCount;
        private string _FullName;
        private bool _IsFolder;
        private DateTime _LastAccessDate;
        private DateTime _LastWriteDate;
        private string _Name;
        private long _Size;
        private int _SubFolderCount;

        #endregion

        #region 公有属性

        /// <summary>
        ///     名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        ///     文件或目录的完整目录
        /// </summary>
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }

        public bool IsFolder
        {
            get { return _IsFolder; }
            set { _IsFolder = value; }
        }

        /// <summary>
        ///     文件大小
        /// </summary>
        public long Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        /// <summary>
        ///     上次访问时间
        /// </summary>
        public DateTime LastAccessDate
        {
            get { return _LastAccessDate; }
            set { _LastAccessDate = value; }
        }

        /// <summary>
        ///     上次读写时间
        /// </summary>
        public DateTime LastWriteDate
        {
            get { return _LastWriteDate; }
            set { _LastWriteDate = value; }
        }

        /// <summary>
        ///     文件个数
        /// </summary>
        public int FileCount
        {
            get { return _FileCount; }
            set { _FileCount = value; }
        }

        /// <summary>
        ///     目录个数
        /// </summary>
        public int SubFolderCount
        {
            get { return _SubFolderCount; }
            set { _SubFolderCount = value; }
        }

        #endregion
    }
}