using System;
using System.Collections;
using System.Collections.Generic;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the collection entity for objects inheriting the <see cref="LinkedResource" /> class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinkedResourceCollection<T> : LinkedResource, ICollection<T> where T : LinkedResource
    {
        private readonly List<T> _items;

        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResourceCollection{T}" /> class.
        /// </summary>
        public LinkedResourceCollection()
        {
            this._items = new List<T>();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResourceCollection{T}" /> class.
        /// </summary>
        /// <param name="items">List of items inheriting the <see cref="LinkedResource" /> class.</param>
        public LinkedResourceCollection(List<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            this._items = items;
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count
        {
            get { return this._items.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only. This always returns <c>False</c>.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Returns an enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Returns an enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            this._items.Add(item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            this._items.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains a specific item or not.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            var result = this._items.Contains(item);
            return result;
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this._items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The object to remove from the collection.</param>
        /// <returns>
        /// Returns <c>True</c>, if the item was successfully removed from the collection; otherwise, returns <c>False</c>. This method also returns <c>False</c>, if item is not found in the original collection.
        /// </returns>
        public bool Remove(T item)
        {
            var result = this._items.Remove(item);
            return result;
        }
    }
}