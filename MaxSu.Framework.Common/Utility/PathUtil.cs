using System.IO;

namespace MaxSu.Framework.Common.Utility
{
	public static class PathUtil
	{
		public static string GetPathAndFileNameWithoutExtension(string file) {
			var fn = Path.GetFileNameWithoutExtension(file);
			var path = Path.GetDirectoryName(file);
			var ret = "";
			if (path != "")
				ret = path + Path.DirectorySeparatorChar + fn;
			else
				ret = fn;

			return ret;
		}
	}
}
