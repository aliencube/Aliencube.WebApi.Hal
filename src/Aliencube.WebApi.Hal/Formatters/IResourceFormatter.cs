using System;
using System.IO;
using System.Net.Http;
using System.Text;

using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This provides interfaces to the resource formatter class.
    /// </summary>
    public interface IResourceFormatter : IDisposable
    {
        /// <summary>
        /// Writes the value to the output stream by serialising in JSON.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="effectiveEncoding">The encoding to use when writing.</param>
        void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding);

        /// <summary>
        /// Writes the value to the output stream by serialising in XML.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="content">The <see cref="HttpContent" /> instance, if available. This can be null.</param>
        void WriteToStream(Type type, object value, Stream writeStream, HttpContent content);

        /// <summary>
        /// Called during the <see cref="LinkedResource" /> serialisation.
        /// </summary>
        /// <param name="resource"><see cref="LinkedResource" /> instance.</param>
        /// <param name="objects">List of objects for serialisation.</param>
        void OnSerialisingResource(LinkedResource resource, params object[] objects);
    }
}