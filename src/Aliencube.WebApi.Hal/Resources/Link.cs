using System.Collections.Generic;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for link.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Link" /> class.
        /// </summary>
        public Link()
        {
            this.OptionalParameters = new List<OptionalParameter>();
        }

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

        /// <summary>
        /// Gets a value indicating whether whether the <c>Href</c> is templated URI or not.
        /// </summary>
        [JsonProperty(PropertyName = "templated")]
        public bool IsHrefTemplated
        {
            get { return this.Href.Contains("{") && this.Href.Contains("}"); }
        }

        /// <summary>
        /// Gets the list of <see cref="OptionalParameter" /> instances.
        /// </summary>
        [JsonIgnore]
        public List<OptionalParameter> OptionalParameters { get; set; }

        /// <summary>
        /// Checks the value whether the <c>IsHrefTemplated</c> value is <c>True</c> so that the property should be included for serialisation or not.
        /// </summary>
        /// <returns>
        /// Returns <c>True</c>, if the <c>IsHrefTemplated</c> is <c>True</c>; otherwise returns <c>False</c>.
        /// </returns>
        public bool ShouldSerializeIsHrefTemplated()
        {
            return this.IsHrefTemplated;
        }
    }
}