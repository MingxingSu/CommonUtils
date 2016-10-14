using System;
using System.Text;

namespace MaxSu.Framework.Common.Extensions
{
    public static class ByteArrayExtensions
    {
        // Methods
        public static bool AreEqual(byte[] bytes1, byte[] bytes2)
        {
            if (!ReferenceEquals(bytes1, null) || !ReferenceEquals(bytes2, null))
            {
                if (ReferenceEquals(bytes1, null) || ReferenceEquals(bytes2, null))
                {
                    return false;
                }
                if (bytes1.Length != bytes2.Length)
                {
                    return false;
                }
                for (int i = 0; i < bytes1.Length; i++)
                {
                    if (bytes1[i] != bytes2[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int Compare(byte[] bytes1, byte[] bytes2)
        {
            if (ReferenceEquals(bytes1, null) && ReferenceEquals(bytes2, null))
            {
                return 0;
            }
            if (ReferenceEquals(bytes1, null))
            {
                return -1;
            }
            if (ReferenceEquals(bytes2, null))
            {
                return 1;
            }
            int length1 = bytes1.Length;
            int length2 = bytes2.Length;
            for (int i = 0; (i < length1) && (i < length2); i++)
            {
                int comparison = bytes1[i].CompareTo(bytes2[i]);
                if (comparison != 0)
                {
                    return comparison;
                }
            }
            return length1.CompareTo(length2);
        }

        public static string ToHexString(this byte[] bytes)
        {
            return bytes.ToHexString(null);
        }

        public static string ToHexString(this byte[] bytes, string format)
        {
            ByteHexFormat byteHexFormat;
            if (string.IsNullOrEmpty(format) || (format == "n"))
            {
                byteHexFormat = ByteHexFormat.NoSpacing;
            }
            else if (format == "s")
            {
                byteHexFormat = ByteHexFormat.Spaces;
            }
            else
            {
                if (format != "r")
                {
                    throw new ArgumentException("Unsupported format string", format);
                }
                byteHexFormat = ByteHexFormat.Rows;
            }
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                if ((byteHexFormat != ByteHexFormat.NoSpacing) && (builder.Length > 0))
                {
                    if ((byteHexFormat == ByteHexFormat.Rows) && ((i%0x10) == 0))
                    {
                        builder.Append('\n');
                    }
                    else
                    {
                        builder.Append(' ');
                    }
                }
                builder.AppendFormat("{0:X2}", bytes[i]);
            }
            return builder.ToString();
        }

        // Nested Types
        private enum ByteHexFormat
        {
            NoSpacing,
            Spaces,
            Rows
        }
    }
}