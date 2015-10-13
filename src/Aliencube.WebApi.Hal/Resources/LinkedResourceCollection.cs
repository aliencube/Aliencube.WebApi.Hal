using System.Collections.Generic;

namespace Aliencube.WebApi.App.Resources
{
    /// <summary>
    /// This represents the collection entity for the <see cref="LinkedResource" /> class.
    /// </summary>
    public class LinkedResourceCollection
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResourceCollection" /> class.
        /// </summary>
        public LinkedResourceCollection()
        {
            this.Items = new List<LinkedResource>();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResourceCollection" /> class.
        /// </summary>
        /// <param name="resources">List of the <see cref="LinkedResource" /> objects to add.</param>
        public LinkedResourceCollection(List<LinkedResource> resources)
        {
            this.Items = resources;
        }

        /// <summary>
        /// Gets the list of <see cref="LinkedResource" /> items.
        /// </summary>
        public List<LinkedResource> Items { get; private set; }

        /// <summary>
        /// Adds the <see cref="LinkedResource" /> object to the collection.
        /// </summary>
        /// <param name="resource">The <see cref="LinkedResource" /> object to add.</param>
        public void Add(LinkedResource resource)
        {
            this.Items.Add(resource);
        }

        /// <summary>
        /// Adds the list of <see cref="LinkedResource" /> objects to the collection.
        /// </summary>
        /// <param name="resources">List of the <see cref="LinkedResource" /> objects to add.</param>
        public void AddRange(IEnumerable<LinkedResource> resources)
        {
            this.Items.AddRange(resources);
        }
    }
}