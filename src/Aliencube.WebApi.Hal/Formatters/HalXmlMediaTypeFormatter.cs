using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using Aliencube.WebApi.Hal.Helpers;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the formatter entity to support XML response format with HAL.
    /// </summary>
    public class HalXmlMediaTypeFormatter : BufferedMediaTypeFormatter
    {
        private XmlWriterSettings _settings;

        /// <summary>
        /// Initialises a new instance of the <see cref="HalXmlMediaTypeFormatter" /> class.
        /// </summary>
        public HalXmlMediaTypeFormatter()
        {
            this._settings = new XmlWriterSettings()
                             {
                                 Indent = true,
                                 OmitXmlDeclaration = false,
                                 Encoding = Encoding.UTF8
                             };

            this.SetSupportedMediaTypes();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="HalXmlMediaTypeFormatter" /> class.
        /// </summary>
        /// <param name="ns">Default namespace.</param>
        public HalXmlMediaTypeFormatter(string ns)
            : this()
        {
            this.Namespace = ns;
        }

        /// <summary>
        /// Gets or sets the default namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Determines whether the <see cref="HalXmlMediaTypeFormatter" /> can read objects of the specified type.
        /// </summary>
        /// <param name="type">The type of object that will be read.</param>
        /// <returns>
        /// Returns <c>True</c>, if objects of this type can be read, otherwise returns <c>False</c>.
        /// </returns>
        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            var isSupportedType = FormatterHelper.IsSupportedType(type);
            return isSupportedType;
        }

        /// <summary>
        /// Determines whether the <see cref="HalXmlMediaTypeFormatter" /> can write objects of the specified type.
        /// </summary>
        /// <param name="type">The type of object that will be written.</param>
        /// <returns>
        /// Returns <c>True</c>, if objects of this type can be written, otherwise returns <c>False</c>.
        /// </returns>
        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            var isSupportedType = FormatterHelper.IsSupportedType(type);
            return isSupportedType;
        }

        /// <summary>
        /// Called during serialization to write an object of the specified type to the specified stream.
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

            var formatter = XmlResourceFormatter.Create(type, this.Namespace, this._settings);
            formatter.WriteToStream(type, value, writeStream, content);
        }

        private void SetSupportedMediaTypes()
        {
            this.SupportedMediaTypes.Clear();
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+xml"));
        }
    }
}