using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Aliencube.WebApi.Hal.Tests.Helpers
{
    /// <summary>
    /// This represents the helper entity for formatter.
    /// </summary>
    public static class FormatterHelper
    {
        /// <summary>
        /// Parses contents from stream to JSON object.
        /// </summary>
        /// <param name="stream">Stream containing value.</param>
        /// <returns>Returns JSON object parsed.</returns>
        public static JObject ParseJsonStream(Stream stream)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();
                var jo = JObject.Parse(result);
                return jo;
            }
        }

        /// <summary>
        /// Parses contents from stream to XML document.
        /// </summary>
        /// <param name="stream">Stream containing value.</param>
        /// <returns>Returns XML document parsed.</returns>
        public static XDocument ParseXmlStream(Stream stream)
        {
            stream.Position = 0;
            var doc = XDocument.Load(stream);
            return doc;
        }
    }
}