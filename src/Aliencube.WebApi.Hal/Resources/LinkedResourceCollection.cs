using System.Collections;
using System.Collections.Generic;

namespace Aliencube.WebApi.Hal.Resources
{
    public class LinkedResourceCollection<T> : LinkedResource, ICollection<T> where T : LinkedResource
    {
        private readonly List<T> _items;

        public LinkedResourceCollection()
        {
            this._items = new List<T>();
        }

        public int Count
        {
            get { return this._items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        public void Add(T item)
        {
            this._items.Add(item);
        }

        public void Clear()
        {
            this._items.Clear();
        }

        public bool Contains(T item)
        {
            var result = this._items.Contains(item);
            return result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this._items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var result = this._items.Remove(item);
            return result;
        }
    }
}