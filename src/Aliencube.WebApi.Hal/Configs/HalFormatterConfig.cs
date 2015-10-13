using System.Web.Http;

using Aliencube.WebApi.Hal.Formatters;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aliencube.WebApi.Hal.Configs
{
    /// <summary>
    /// This represents the extension entity for HAL formatter configuration.
    /// </summary>
    public static class HalFormatterConfig
    {
        /// <summary>
        /// Configures HAL formatters.
        /// </summary>
        /// <param name="config">
        /// The <see cref="HttpConfiguration" /> instance.
        /// </param>
        public static void ConfigHalFormatter(this HttpConfiguration config)
        {
            var jsonFormatter = new HalJsonMediaTypeFormatter()
                                    {
                                        SerializerSettings = new JsonSerializerSettings()
                                                                 {
                                                                     ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                     MissingMemberHandling = MissingMemberHandling.Ignore,
                                                                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                                                     NullValueHandling = NullValueHandling.Ignore,
                                                                 },
                                    };

            //var xmlFormatter = new HalXmlMediaTypeFormatter()
            //                       {
            //                           Namespace = "http://schema.aliencube.org/xml/2015/08/hal",
            //                       };

            config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Insert(0, jsonFormatter);
            //config.Formatters.Insert(1, xmlFormatter);
        }
    }
}
