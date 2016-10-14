using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MaxSu.Framework.Common.Collections
{
	[Serializable]
	public class DictionaryList<TKey, TValue> : Dictionary<TKey, List<TValue>>, ISerializable
	{
		public DictionaryList() { }

		protected DictionaryList(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public void Add(TKey key, TValue value) {
			if (!base.ContainsKey(key))
				base.Add(key, new List<TValue>());

			base[key].Add(value);
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context) { base.GetObjectData(info, context); }

		
	}

}
