using System;
using System.IO;
using System.Text;

using Aliencube.WebApi.Hal.Resources;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the formatter entity for the <see cref="LinkedResourceCollection{T}" />.
    /// </summary>
    public class JsonLinkedResourceCollectionFormatter : JsonResourceFormatter
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="JsonLinkedResourceCollectionFormatter" /> class.
        /// </summary>
        /// <param name="settings">The <see cref="JsonSerializerSettings" /> value.</param>
        public JsonLinkedResourceCollectionFormatter(JsonSerializerSettings settings)
            : base(settings)
        {
        }

        /// <summary>
        /// Writes the value to the output stream.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="effectiveEncoding">The encoding to use when writing.</param>
        public override void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding)
        {
            throw new NotImplementedException();
        }
    }
}