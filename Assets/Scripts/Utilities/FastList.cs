using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class FastList<T> : IEnumerable<T>
    {
        private List<T> itemList;
        Dictionary<T, int> indexByItem;

        public int Count { get => itemList.Count; }

        public FastList()
        {
            itemList = new List<T>();
            indexByItem = new Dictionary<T, int>();
        }

        public void Add(T item)
        {
            if (indexByItem.ContainsKey(item))
            {
                Debug.LogError("Item already exists in list.");
                return;
            }

            int index = itemList.Count;
            itemList.Add(item);

            indexByItem[item] = index;
        }

        public void Remove(T item)
        {
            int index;
            if (!indexByItem.TryGetValue(item, out index))
            {
                Debug.LogError("Item not found in list.");
                return;
            }
            
            indexByItem.Remove(item);

            int last = itemList.Count - 1;
            // Special case: removing last element
            if (index == last)
            {
                itemList.RemoveAt(last);
            }
            else
            {
                T lastItem = itemList[last];
                itemList[index] = lastItem;
                itemList.RemoveAt(last);

                indexByItem[lastItem] = index;
            }
        }

        public bool Contains(T item)
        {
            return indexByItem.ContainsKey(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in itemList)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}