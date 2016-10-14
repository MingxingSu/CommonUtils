using System.Text;

namespace MaxSu.Framework.Common.Extensions
{
	public static class StringBuilderEx
	{
		public static void RemoveLastChar(this StringBuilder sb) {
			if (sb.Length >= 1)
				sb.Remove(sb.Length - 1, 1);
		}
	}
}
