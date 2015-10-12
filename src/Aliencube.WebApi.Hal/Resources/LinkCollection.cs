using System.Collections.Generic;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the collection entity for the <see cref="Link" /> class.
    /// </summary>
    public class LinkCollection
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LinkCollection" /> class.
        /// </summary>
        public LinkCollection()
        {
            this.Items = new List<Link>();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="LinkCollection" /> class.
        /// </summary>
        /// <param name="links">List of the <see cref="Link" /> objects to add.</param>
        public LinkCollection(List<Link> links)
        {
            this.Items = new List<Link>();
        }

        /// <summary>
        /// Gets the list of <see cref="Link" /> items.
        /// </summary>
        public List<Link> Items { get; }

        /// <summary>
        /// Adds the <see cref="Link" /> object to the collection.
        /// </summary>
        /// <param name="link">The <see cref="Link" /> object to add.</param>
        public void Add(Link link)
        {
            this.Items.Add(link);
        }

        /// <summary>
        /// Adds the list of <see cref="Link" /> objects to the collection.
        /// </summary>
        /// <param name="links">List of the <see cref="Link" /> objects to add.</param>
        public void AddRange(IEnumerable<Link> links)
        {
            this.Items.AddRange(links);
        }
    }
}
