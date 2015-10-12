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
        /// Gets the <see cref="LinkCollection" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public LinkCollection Links { get; }

        /// <summary>
        /// Gets the <see cref="HalResourceCollection" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_embedded")]
        public HalResourceCollection Embedded { get; }
    }
}