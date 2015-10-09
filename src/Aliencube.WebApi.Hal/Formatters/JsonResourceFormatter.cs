using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

using Aliencube.WebApi.Hal.Helpers;
using Aliencube.WebApi.Hal.Resources;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the resource formatter entity. This must be inherited.
    /// </summary>
    public abstract class JsonResourceFormatter : IResourceFormatter
    {
        private bool _disposed;

        /// <summary>
        /// Initialises a new instance of the <see cref="JsonResourceFormatter" /> class.
        /// </summary>
        protected JsonResourceFormatter()
        {
            this.Settings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="JsonResourceFormatter" /> class.
        /// </summary>
        /// <param name="settings">The <see cref="JsonSerializerSettings" /> value.</param>
        protected JsonResourceFormatter(JsonSerializerSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Gets the <see cref="JsonSerializerSettings" /> value.
        /// </summary>
        protected JsonSerializerSettings Settings { get; }

        /// <summary>
        /// Creates the <see cref="IResourceFormatter" /> instance.
        /// </summary>
        /// <param name="type"><see cref="Type" /> to check.</param>
        /// <param name="settings"><see cref="JsonSerializerSettings" /> instance.</param>
        /// <returns>Returns the <see cref="IResourceFormatter" /> instance created.</returns>
        public static IResourceFormatter Create(Type type, JsonSerializerSettings settings)
        {
            IResourceFormatter formatter;
            if (FormatterHelper.IsLinkedResourceCollectionType(type))
            {
                formatter = new JsonLinkedResourceCollectionFormatter(settings);
            }
            else
            {
                formatter = new JsonLinkedResourceFormatter(settings);
            }

            return formatter;
        }

        /// <summary>
        /// Writes the value to the output stream by serialising in JSON.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="effectiveEncoding">The encoding to use when writing.</param>
        public abstract void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding);

        /// <summary>
        /// Writes the value to the output stream by serialising in XML.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="content">The <see cref="HttpContent" /> instance, if available. This can be null.</param>
        public virtual void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            throw new NotImplementedException("Not an XML formatter");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;
        }

        /// <summary>
        /// Parses the <see cref="LinkedResource" /> instance.
        /// </summary>
        /// <param name="resource"><see cref="LinkedResource" /> instance to be parsed.</param>
        /// <returns>Returns the JSON parsed resource.</returns>
        protected JObject ParseResource(LinkedResource resource)
        {
            var so = JsonConvert.SerializeObject(resource, this.Settings);

            var jo = JObject.Parse(so);
            return jo;
        }

        /// <summary>
        /// Parses the list of <see cref="Link" /> objects within the <see cref="LinkedResource" /> instance.
        /// </summary>
        /// <param name="resource"><see cref="LinkedResource" /> instance containing the list of <see cref="Link" /> objects.</param>
        /// <returns>Returns the JSON parsed links.</returns>
        protected JObject ParseLinks(LinkedResource resource)
        {
            var links = resource.Links.Select(p => new KeyValuePair<string, Link>(p.Rel, p)).ToList();
            var parsed = ParseLinks(links);

            if (string.IsNullOrWhiteSpace(resource.Href))
            {
                return parsed;
            }

            if (links.Any(p => p.Key.Equals("self", StringComparison.InvariantCultureIgnoreCase)))
            {
                return parsed;
            }

            links.Insert(0, new KeyValuePair<string, Link>("self", new Link() { Rel = "self", Href = resource.Href }));

            parsed = ParseLinks(links);
            return parsed;
        }

        /// <summary>
        /// Writes the JSON object to the stream with given encoding.
        /// </summary>
        /// <param name="resource">Parsed JSON object to write.</param>
        /// <param name="stream"><see cref="Stream" /> instance.</param>
        /// <param name="encoding"><see cref="Encoding" /> value.</param>
        protected void WriteToStream(JToken resource, Stream stream, Encoding encoding)
        {
            using (var sw = new StreamWriter(stream, encoding))
            using (var writer = new JsonTextWriter(sw))
            {
                resource.WriteTo(writer);
                writer.Flush();
                sw.Flush();
            }
        }

        private static JObject ParseLinks(IReadOnlyList<KeyValuePair<string, Link>> links)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            for (var i = 0; i < links.Count; i++)
            {
                var link = links[i];
                sb.AppendFormat(
                                "\"{0}\": {1}{2}",
                                link.Key,
                                JsonConvert.SerializeObject(link.Value),
                                i == links.Count - 1 ? string.Empty : ",");
            }

            sb.Append("}");

            var jlo = JObject.Parse(sb.ToString());
            return jlo;
        }
    }
}