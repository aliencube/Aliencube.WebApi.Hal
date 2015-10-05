using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the resource formatter entity. This must be inherited.
    /// </summary>
    public abstract class XmlResourceFormatter : IResourceFormatter
    {
        private bool _disposed;

        /// <summary>
        /// Writes the value to the output stream by serialising in JSON.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="effectiveEncoding">The encoding to use when writing.</param>
        public virtual void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding)
        {
            throw new NotImplementedException("Not a JSON formatter");
        }

        /// <summary>
        /// Writes the value to the output stream by serialising in XML.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="content">The <see cref="HttpContent" /> instance, if available. This can be null.</param>
        public abstract void WriteToStream(Type type, object value, Stream writeStream, HttpContent content);

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
    }
}