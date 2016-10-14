using System;

namespace MaxSu.Framework.Common.Web
{
    /// <summary>
    ///     �������ݲ���
    /// </summary>
    public class DownloadEventArgs : EventArgs
    {
        /// <summary>
        ///     �ѽ��յ��ֽ���
        /// </summary>
        public int BytesReceived { get; set; }

        /// <summary>
        ///     ���ֽ���
        /// </summary>
        public int TotalBytes { get; set; }

        /// <summary>
        ///     ��ǰ���������յ�����
        /// </summary>
        public byte[] ReceivedData { get; set; }
    }
}