using System;

namespace MaxSu.Framework.Common
{
    [Serializable]
    public class FileItem
    {
        #region ˽���ֶ�

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

        #region ��������

        /// <summary>
        ///     ����
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        ///     �ļ���Ŀ¼������Ŀ¼
        /// </summary>
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        /// <summary>
        ///     ����ʱ��
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
        ///     �ļ���С
        /// </summary>
        public long Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        /// <summary>
        ///     �ϴη���ʱ��
        /// </summary>
        public DateTime LastAccessDate
        {
            get { return _LastAccessDate; }
            set { _LastAccessDate = value; }
        }

        /// <summary>
        ///     �ϴζ�дʱ��
        /// </summary>
        public DateTime LastWriteDate
        {
            get { return _LastWriteDate; }
            set { _LastWriteDate = value; }
        }

        /// <summary>
        ///     �ļ�����
        /// </summary>
        public int FileCount
        {
            get { return _FileCount; }
            set { _FileCount = value; }
        }

        /// <summary>
        ///     Ŀ¼����
        /// </summary>
        public int SubFolderCount
        {
            get { return _SubFolderCount; }
            set { _SubFolderCount = value; }
        }

        #endregion
    }
}