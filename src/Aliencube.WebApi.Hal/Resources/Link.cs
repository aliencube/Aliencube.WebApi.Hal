using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for link.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Gets or sets the rel of the link.
        /// </summary>
        [JsonIgnore]
        public string Rel { get; set; }

        /// <summary>
        /// Gets or sets the href of the link.
        /// </summary>
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
    }
}