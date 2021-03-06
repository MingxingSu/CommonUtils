﻿using System;
using System.Collections.Generic;

namespace MaxSu.Framework.Common.Collections.Generic
{
    public sealed partial class BinarySearchTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            return this.InOrderIterator.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.InOrderIterator.GetEnumerator();
        }
    }
}
