using System.Collections.Generic;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for HAL resource. This must be inherited.
    /// </summary>
    public abstract class HalResource
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HalResource" /> class.
        /// </summary>
        protected HalResource()
        {
            this.Links = new LinkCollection();
        }

        /// <summary>
        /// Gets the list of <see cref="Link" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public LinkCollection Links { get; }

        /// <summary>
        /// Gets the list of <see cref="LinkedResource" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_embedded")]
        public List<LinkedResource> Embedded { get; }
    }
}