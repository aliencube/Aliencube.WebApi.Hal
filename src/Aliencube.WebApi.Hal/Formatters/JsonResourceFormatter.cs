using System;
using System.IO;
using System.Net.Http;
using System.Text;

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
        /// <param name="settings">The <see cref="JsonSerializerSettings" /> value.</param>
        protected JsonResourceFormatter(JsonSerializerSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Gets the <see cref="JsonSerializerSettings" /> value.
        /// </summary>
        protected JsonSerializerSettings Settings { get; private set; }

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

        protected JObject SerialiseResource(object embedded)
        {
            var so = this.SerializerSettings == null
                ? JsonConvert.SerializeObject(embedded, this.Formatting)
                : JsonConvert.SerializeObject(embedded, this.Formatting, this.SerializerSettings);

            var jo = JObject.Parse(so);
            return jo;
        }
    }
}