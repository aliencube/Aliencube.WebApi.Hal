using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Xml;

using Aliencube.WebApi.Hal.Extensions;
using Aliencube.WebApi.Hal.Helpers;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the formatter entity to support XML response format with HAL.
    /// </summary>
    public class HalXmlMediaTypeFormatter : BufferedMediaTypeFormatter
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HalXmlMediaTypeFormatter" /> class.
        /// </summary>
        public HalXmlMediaTypeFormatter()
        {
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
            var resource = value as LinkedResource;
            if (resource == null)
            {
                return;
            }

            var settings = new XmlWriterSettings()
                           {
                               Indent = true,
                               OmitXmlDeclaration = false,
                               Encoding = Encoding.UTF8
                           };

            using (var writer = XmlWriter.Create(writeStream, settings))
            {
                if (string.IsNullOrWhiteSpace(this.Namespace))
                {
                    writer.WriteStartElement("resource");
                }
                else
                {
                    writer.WriteStartElement("resource", this.Namespace);
                }

                SetSelfLink(resource);
                SerialiseLinks(writer, resource.Links);
                SerialiseResources(writer, type, resource);

                writer.WriteEndElement();
                writer.Flush();
            }
        }

        private static void SetSelfLink(LinkedResource resource)
        {
            if (string.IsNullOrWhiteSpace(resource.Href))
            {
                return;
            }

            var links = resource.Links;
            if (links.Any(p => p.Rel.Equals("self", StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            resource.Links.Insert(0, new Link() { Rel = "self", Href = resource.Href });
        }

        private static void SerialiseResources(XmlWriter writer, Type type, LinkedResource resource)
        {
            if (FormatterHelper.IsLinkedResourceCollectionType(type))
            {
                writer.WriteStartElement("resources");

                foreach (LinkedResource innerResource in (IEnumerable)resource)
                {
                    SerialiseInnerResource(writer, innerResource);
                }

                writer.WriteEndElement();
            }
            else
            {
                SerialiseInnerResource(writer, resource);
            }
        }

        private static void SerialiseInnerResource(XmlWriter writer, LinkedResource resource)
        {
            writer.WriteStartElement("resource");

            SetSelfLink(resource);
            SerialiseLinks(writer, resource.Links);

            var properties = resource.GetType()
                                     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Where(p => !p.Name.Equals("rel", StringComparison.InvariantCultureIgnoreCase) &&
                                                 !p.Name.Equals("href", StringComparison.InvariantCultureIgnoreCase));

            SerialiseProperties(writer, resource, properties);

            writer.WriteEndElement();
        }

        private static void SerialiseLinks(XmlWriter writer, IEnumerable<Link> links)
        {
            writer.WriteStartElement("links");

            foreach (var link in links)
            {
                writer.WriteStartElement("link");

                writer.WriteElementString("rel", link.Rel);
                writer.WriteElementString("href", link.Href);

                if (link.Href.Contains("{") && link.Href.Contains("}"))
                {
                    writer.WriteElementString("templated", true.ToString().ToLowerInvariant());
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private static void SerialiseProperties<T>(XmlWriter writer, T resource, IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(resource);
                if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                {
                    writer.WriteElementString(property.Name.ToCamelCase(), propertyValue.ToString());
                }
                else
                {
                    var value = propertyValue as LinkedResource;
                    if (value != null)
                    {
                        SerialiseInnerResource(writer, value);
                    }
                }
            }
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