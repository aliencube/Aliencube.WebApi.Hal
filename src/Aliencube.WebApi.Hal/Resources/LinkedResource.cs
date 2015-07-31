using System.Collections.Generic;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for resources containing links. This must be inherited.
    /// </summary>
    public abstract class LinkedResource : Link
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResource" /> class.
        /// </summary>
        protected LinkedResource()
        {
            this.Links = new List<Link>();
        }

        /// <summary>
        /// Gets or sets the list of <see cref="Link" /> objects.
        /// </summary>
        public List<Link> Links { get; private set; }
    }
}