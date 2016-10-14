using System;

namespace MaxSu.Framework.Common.Web
{
    /// <summary>
    ///     下载数据参数
    /// </summary>
    public class DownloadEventArgs : EventArgs
    {
        /// <summary>
        ///     已接收的字节数
        /// </summary>
        public int BytesReceived { get; set; }

        /// <summary>
        ///     总字节数
        /// </summary>
        public int TotalBytes { get; set; }

        /// <summary>
        ///     当前缓冲区接收的数据
        /// </summary>
        public byte[] ReceivedData { get; set; }
    }
}