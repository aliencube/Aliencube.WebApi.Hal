using System.Collections.Generic;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for resources containing links. This must be inherited.
    /// </summary>
    public abstract class LinkedResource
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResource" /> class.
        /// </summary>
        protected LinkedResource()
        {
            this.Links = new List<Link>();
        }

        /// <summary>
        /// Gets or sets the href of the link.
        /// </summary>
        /// <remarks>This value is injected during the serialisation as "self", unless <c>Links</c> contains it.</remarks>
        [JsonIgnore]
        public string Href { get; set; }

        /// <summary>
        /// Gets the list of <see cref="Link" /> objects.
        /// </summary>
        [JsonIgnore]
        public List<Link> Links { get; private set; }
    }
}