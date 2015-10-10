using System;
using System.Collections;
using System.IO;
using System.Text;

using Aliencube.WebApi.Hal.Resources;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the JSON formatter entity for the <see cref="LinkedResourceCollection{T}" />.
    /// </summary>
    public class JsonLinkedResourceCollectionFormatter : JsonResourceFormatter
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="JsonLinkedResourceCollectionFormatter" /> class.
        /// </summary>
        public JsonLinkedResourceCollectionFormatter()
            : base()
        {
        }

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

            this.WriteToStream(writeStream, effectiveEncoding, resource);
        }

        /// <summary>
        /// Called during the <see cref="LinkedResource" /> serialisation.
        /// </summary>
        /// <param name="resource"><see cref="LinkedResource" /> instance.</param>
        /// <param name="objects">List of objects for serialisation.</param>
        public override void OnSerialisingResource(LinkedResource resource, params object[] objects)
        {
            if (objects.Length > 2)
            {
                throw new InvalidOperationException("Too many parameters");
            }

            var writeStream = objects[0] as Stream;
            if (writeStream == null)
            {
                throw new InvalidOperationException("Invalid parameter");
            }

            var effectiveEncoding = objects[1] as Encoding;
            if (effectiveEncoding == null)
            {
                throw new InvalidOperationException("Invalid parameter");
            }

            var resources = new LinkedResource[((ICollection)resource).Count];
            ((ICollection)resource).CopyTo(resources, 0);
            var parsedCollection = this.ParseResourceCollection(resources);

            var parsedLinks = this.ParseLinks(resource);

            var parsedResource = new JObject();
            parsedResource["_embedded"] = parsedCollection;
            parsedResource["_links"] = parsedLinks;

            this.WriteToStream(parsedResource, writeStream, effectiveEncoding);
        }
    }
}