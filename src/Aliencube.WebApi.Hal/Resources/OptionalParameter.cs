using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for optional parameter used in the <see cref="Link" /> class.
    /// </summary>
    public class OptionalParameter
    {
        [JsonIgnore]
        public OptionalParameterType Key { get; set; }

        [JsonIgnore]
        public string Value { get; set; }
    }
}