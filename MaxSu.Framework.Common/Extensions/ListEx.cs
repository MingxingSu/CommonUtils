using System.Collections.Generic;

namespace MaxSu.Framework.Common.Extensions
{
	public static class ListEx
	{
		public static void AutoFillDefault<T>(this List<T> self, int count) {
			for (int x = 0; x < count; ++x) {
				self.Add(default(T));
			}
		}
	}
}
