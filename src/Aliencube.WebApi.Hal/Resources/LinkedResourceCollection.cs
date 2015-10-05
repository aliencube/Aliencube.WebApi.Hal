using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the collection entity for objects inheriting the <see cref="LinkedResource" /> class.
    /// </summary>
    /// <typeparam name="T">Type that inherits <see cref="LinkedResource" /> class.</typeparam>
    public class LinkedResourceCollection<T> : LinkedResource where T : LinkedResource
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResourceCollection{T}" /> class.
        /// </summary>
        public LinkedResourceCollection()
        {
            this.Items = new List<T>();
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

            this.Items = items;
        }

        /// <summary>
        /// Gets the list of <see cref="LinkedResource" /> items.
        /// </summary>
        [JsonProperty(PropertyName = "_embedded")]
        public List<T> Items { get; private set; }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        [JsonIgnore]
        public int Count
        {
            get { return this.Items.Count; }
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(T item)
        {
            this.Items.Add(item);
        }

        /// <summary>
        /// Adds a list of items to the collection.
        /// </summary>
        /// <param name="items">List of items to add. </param>
        public void AddRange(IEnumerable<T> items)
        {
            this.Items.AddRange(items);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            this.Items.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains a specific item or not.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>Returns <c>True</c>, if the collection contains the item; otherwise returns <c>False</c>.</returns>
        public bool Contains(T item)
        {
            var result = this.Items.Contains(item);
            return result;
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
            var result = this.Items.Remove(item);
            return result;
        }
    }
}