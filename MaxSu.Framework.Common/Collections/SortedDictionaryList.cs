﻿using System.Collections.Generic;

namespace MaxSu.Framework.Common.Collections
{
	//[Serializable]
	public class SortedDictionaryList<TKey, TValue> : SortedDictionary<TKey, List<TValue>>/*,ISerializable*/
	{
		public SortedDictionaryList() { }

		//protected SortedDictionaryList(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public void Add(TKey key, TValue value) {
			if (!base.ContainsKey(key))
				base.Add(key, new List<TValue>());

			base[key].Add(value);
		}

		//public virtual void GetObjectData(SerializationInfo info, StreamingContext context) { base.GetObjectData(info, context); }
	}
}
