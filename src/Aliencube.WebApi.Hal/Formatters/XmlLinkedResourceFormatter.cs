using System;
using System.IO;
using System.Net.Http;
using System.Xml;

using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the XML formatter entity for the <see cref="LinkedResource" />.
    /// </summary>
    public class XmlLinkedResourceFormatter : XmlResourceFormatter
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="XmlLinkedResourceFormatter" /> class.
        /// </summary>
        public XmlLinkedResourceFormatter()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="XmlLinkedResourceFormatter" /> class.
        /// </summary>
        /// <param name="ns">The namespace for XML.</param>
        public XmlLinkedResourceFormatter(string ns)
            : base(ns)
        {
        }

        /// <summary>
        /// Writes the value to the output stream by serialising in XML.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="content">The <see cref="HttpContent" /> instance, if available. This can be null.</param>
        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
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

            var resource = value as LinkedResource;
            if (resource == null)
            {
                return;
            }

            this.WriteToStream(type, writeStream, resource);
        }

        /// <summary>
        /// Called during the <see cref="LinkedResource" /> serialisation.
        /// </summary>
        /// <param name="writer"><see cref="XmlWriter" /> instance.</param>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="resource"><see cref="LinkedResource" /> instance.</param>
        public override void OnSerialisingResource(XmlWriter writer, Type type, LinkedResource resource)
        {
            this.SerialiseProperties(writer, resource);
        }
    }
}