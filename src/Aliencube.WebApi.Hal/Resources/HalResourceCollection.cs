using System.Collections.Generic;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the collection entity for the <see cref="HalResource" /> class.
    /// </summary>
    public class HalResourceCollection
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HalResourceCollection" /> class.
        /// </summary>
        public HalResourceCollection()
        {
            this.Items = new List<HalResource>();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="HalResourceCollection" /> class.
        /// </summary>
        /// <param name="resources">List of the <see cref="HalResource" /> objects to add.</param>
        public HalResourceCollection(List<HalResource> resources)
        {
            this.Items = resources;
        }

        /// <summary>
        /// Gets the list of <see cref="HalResource" /> items.
        /// </summary>
        public List<HalResource> Items { get; set; }

        /// <summary>
        /// Adds the <see cref="HalResource" /> object to the collection.
        /// </summary>
        /// <param name="resource">The <see cref="HalResource" /> object to add.</param>
        public void Add(HalResource resource)
        {
            this.Items.Add(resource);
        }

        /// <summary>
        /// Adds the list of <see cref="HalResource" /> objects to the collection.
        /// </summary>
        /// <param name="resources">List of the <see cref="HalResource" /> objects to add.</param>
        public void AddRange(IEnumerable<HalResource> resources)
        {
            this.Items.AddRange(resources);
        }
    }
}