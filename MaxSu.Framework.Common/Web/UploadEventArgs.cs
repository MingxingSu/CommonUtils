using System;

namespace MaxSu.Framework.Common.Web
{
    /// <summary>
    ///     �ϴ����ݲ���
    /// </summary>
    public class UploadEventArgs : EventArgs
    {
        /// <summary>
        ///     �ѷ��͵��ֽ���
        /// </summary>
        public int BytesSent { get; set; }

        /// <summary>
        ///     ���ֽ���
        /// </summary>
        public int TotalBytes { get; set; }
    }
}