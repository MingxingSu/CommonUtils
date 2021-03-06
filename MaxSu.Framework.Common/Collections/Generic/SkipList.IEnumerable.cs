﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace MaxSu.Framework.Common.Collections.Generic
{
    public sealed partial class SkipList<T> : IEnumerable<T> where T : IComparable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            SkipListNode<T> current = this.Head.Next[0];
            while (current != null)
            {
                yield return current.Value;
                current = current.Next[0];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
