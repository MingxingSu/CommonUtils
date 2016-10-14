using System;
using System.Diagnostics;

namespace MaxSu.Framework.Common.Utility
{
	public static class ProcessUtil
	{
		public static bool IsRunning(string processName) {
			if (String.IsNullOrWhiteSpace(processName))
				return false;

			processName = processName.ToLower();
			if (processName.EndsWith(".exe"))
				processName = processName.Substring(0, processName.Length - 4);

			foreach (var p in Process.GetProcesses())
				if (p.ProcessName.ToLower().Equals(processName))
					return true;

			return false;
		}
	}
}
