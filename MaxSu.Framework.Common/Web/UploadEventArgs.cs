using System;

namespace MaxSu.Framework.Common.Web
{
    /// <summary>
    ///     上传数据参数
    /// </summary>
    public class UploadEventArgs : EventArgs
    {
        /// <summary>
        ///     已发送的字节数
        /// </summary>
        public int BytesSent { get; set; }

        /// <summary>
        ///     总字节数
        /// </summary>
        public int TotalBytes { get; set; }
    }
}