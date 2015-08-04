using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using Aliencube.WebApi.Hal.Helpers;
using Aliencube.WebApi.Hal.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This repressents the formatter entity to support JSON response format with HAL.
    /// </summary>
    public class HalJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HalJsonMediaTypeFormatter" /> class.
        /// </summary>
        public HalJsonMediaTypeFormatter()
        {
            this.SetSupportedMediaTypes();
            this.EmbeddedType = EmbeddedType.None;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="HalJsonMediaTypeFormatter" /> class.
        /// </summary>
        /// <param name="embeddedType">
        /// <see cref="EmbeddedType" /> value to be used during the serialisation.
        /// </param>
        public HalJsonMediaTypeFormatter(EmbeddedType embeddedType)
            : this()
        {
            this.EmbeddedType = embeddedType;
        }

        /// <summary>
        /// Gets or sets the <see cref="EmbeddedType" /> value to be used during the serialisation.
        /// </summary>
        /// <remarks>
        /// This value is ignored, if object to serialise inherits <see cref="LinkedResourceCollection{T}" />.
        /// </remarks>
        public EmbeddedType EmbeddedType { get; set; }

        /// <summary>
        /// Determines whether the <see cref="HalJsonMediaTypeFormatter" /> can read objects of the specified type.
        /// </summary>
        /// <param name="type">The type of object that will be read.</param>
        /// <returns>
        /// Returns <c>True</c>, if objects of this type can be read, otherwise returns <c>False</c>.
        /// </returns>
        public override bool CanReadType(Type type)
        {
            var isSupportedType = FormatterHelper.IsSupportedType(type);
            return isSupportedType;
        }

        /// <summary>
        /// Determines whether the <see cref="HalJsonMediaTypeFormatter" /> can write objects of the specified type.
        /// </summary>
        /// <param name="type">The type of object that will be written.</param>
        /// <returns>
        /// Returns <c>True</c>, if objects of this type can be written, otherwise returns <c>False</c>.
        /// </returns>
        public override bool CanWriteType(Type type)
        {
            var isSupportedType = FormatterHelper.IsSupportedType(type);
            return isSupportedType;
        }

        /// <summary>
        /// Called during serialization to write an object of the specified type to the specified stream.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="effectiveEncoding">The encoding to use when writing.</param>
        public override void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding)
        {
            var resource = value as LinkedResource;
            if (resource == null)
            {
                return;
            }

            var embedded = value;
            if (this.UseEmbeddedKey(type))
            {
                embedded = new { _embedded = value };
            }

            var jo = this.SerialiseResource(embedded);

            var links = resource.Links.Select(p => new KeyValuePair<string, object>(p.Rel, p)).ToList();
            var jlo = SerialiseLinks(links);

            jo["_links"] = jlo;

            var sw = new StreamWriter(writeStream, effectiveEncoding);
            var writer = new JsonTextWriter(sw);
            jo.WriteTo(writer);
            writer.Flush();
            sw.Flush();
        }

        private void SetSupportedMediaTypes()
        {
            this.SupportedMediaTypes.Clear();
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
        }

        private bool UseEmbeddedKey(Type type)
        {
            return this.EmbeddedType == EmbeddedType.Embedded || FormatterHelper.IsLinkedResourceCollectionType(type);
        }

        private JObject SerialiseResource(object embedded)
        {
            var formatting = this.Indent ? Formatting.Indented : Formatting.None;
            var so = this.SerializerSettings == null
                ? JsonConvert.SerializeObject(embedded, formatting)
                : JsonConvert.SerializeObject(embedded, formatting, this.SerializerSettings);

            var jo = JObject.Parse(so);
            return jo;
        }

        private static JObject SerialiseLinks(List<KeyValuePair<string, object>> links)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            for (var i = 0; i < links.Count; i++)
            {
                var link = links[i];
                sb.AppendFormat("\"{0}\": {1}{2}",
                                link.Key,
                                JsonConvert.SerializeObject(link.Value),
                                i == links.Count - 1 ? "" : ",");
            }
            sb.Append("}");

            var jlo = JObject.Parse(sb.ToString());
            return jlo;
        }
    }
}