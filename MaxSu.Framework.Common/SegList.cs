using System.Collections;

namespace MaxSu.Framework.Common
{
    /// <summary>
    ///     分词辅助类
    /// </summary>
    public class SegList
    {
        private readonly ArrayList m_seg;
        public int MaxLength;

        public SegList()
        {
            m_seg = new ArrayList();
            MaxLength = 0;
        }

        public int Count
        {
            get { return m_seg.Count; }
        }

        public void Add(object obj)
        {
            m_seg.Add(obj);
            if (MaxLength < obj.ToString().Length)
            {
                MaxLength = obj.ToString().Length;
            }
        }

        public object GetElem(int i)
        {
            if (i < Count)
                return m_seg[i];
            else
                return null;
        }

        public void SetElem(int i, object obj)
        {
            m_seg[i] = obj;
        }

        public bool Contains(object obj)
        {
            return m_seg.Contains(obj);
        }

        /// <summary>
        ///     按长度排序
        /// </summary>
        public void Sort()
        {
            Sort(this);
        }

        /// <summary>
        ///     按长度排序
        /// </summary>
        public void Sort(SegList list)
        {
            int max = 0;
            for (int i = 0; i < list.Count - 1; ++i)
            {
                max = i;
                for (int j = i + 1; j < list.Count; ++j)
                {
                    string str1 = list.GetElem(j).ToString();
                    string str2 = list.GetElem(max).ToString();
                    int l1;
                    int l2;
                    if (str1 == "null")
                        l1 = 0;
                    else
                        l1 = str1.Length;

                    if (str2 == "null")
                        l2 = 0;
                    else
                        l2 = str2.Length;

                    if (l1 > l2)
                        max = j;
                }
                object o = list.GetElem(max);
                list.SetElem(max, list.GetElem(i));
                list.SetElem(i, o);
            }
        }
    }
}