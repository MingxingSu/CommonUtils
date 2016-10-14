using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MaxSu.Framework.Common.Utility;

namespace MaxSu.Framework.Common.Extensions
{
	public static class DictionaryEx
	{
		public static string ToXmlString<K,V>(this IDictionary<K, V> self)  {
			var xmlData = new XElement("Dictionary",
				from key in self.Keys
				select new XElement("Item",
					new XAttribute("Key", key), 
					self[key]
			));

			return StringUtil.XElementToString(xmlData);
		}

		
	}
}
