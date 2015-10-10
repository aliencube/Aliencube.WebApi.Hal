using System;
using System.IO;
using System.Text;

using Aliencube.WebApi.Hal.Resources;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the JSON formatter entity for the <see cref="LinkedResource" />.
    /// </summary>
    public class JsonLinkedResourceFormatter : JsonResourceFormatter
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="JsonLinkedResourceFormatter" /> class.
        /// </summary>
        public JsonLinkedResourceFormatter()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="JsonLinkedResourceFormatter" /> class.
        /// </summary>
        /// <param name="settings">The <see cref="JsonSerializerSettings" /> value.</param>
        public JsonLinkedResourceFormatter(JsonSerializerSettings settings)
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
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (value == null)
            {
                return;
            }

            if (writeStream == null)
            {
                throw new ArgumentNullException(nameof(writeStream));
            }

            if (effectiveEncoding == null)
            {
                throw new ArgumentNullException(nameof(effectiveEncoding));
            }


            var resource = value as LinkedResource;
            if (resource == null)
            {
                return;
            }

            var parsedResource = this.ParseResource(resource);
            var parsedLinks = this.ParseLinks(resource);

            parsedResource["_links"] = parsedLinks;

            this.WriteToStream(parsedResource, writeStream, effectiveEncoding);
        }
    }
}