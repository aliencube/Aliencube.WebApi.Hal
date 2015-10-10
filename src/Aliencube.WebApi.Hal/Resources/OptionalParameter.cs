using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for optional parameter used in the <see cref="Link" /> class.
    /// </summary>
    public class OptionalParameter
    {
        /// <summary>
        /// Gets or sets the <see cref="OptionalParameterType" />.
        /// </summary>
        [JsonIgnore]
        public OptionalParameterType Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [JsonIgnore]
        public string Value { get; set; }
    }
}