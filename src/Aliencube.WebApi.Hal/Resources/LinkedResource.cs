using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for HAL resource. This must be inherited.
    /// </summary>
    public abstract class LinkedResource
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResource" /> class.
        /// </summary>
        protected LinkedResource()
        {
            this.Links = new LinkCollection();
        }

        /// <summary>
        /// Gets or sets the relation of the <see cref="LinkedResource" />.
        /// </summary>
        [JsonIgnore]
        public string Rel { get; set; }

        /// <summary>
        /// Gets or sets the href value of the <see cref="LinkedResource" />.
        /// </summary>
        [JsonIgnore]
        public string Href { get; set; }

        /// <summary>
        /// Gets the <see cref="LinkCollection" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public LinkCollection Links { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="LinkedResourceCollection" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_embedded")]
        public LinkedResourceCollection Embedded { get; set; }
    }
}