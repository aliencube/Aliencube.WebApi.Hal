using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Xml;

using Aliencube.WebApi.Hal.Extensions;
using Aliencube.WebApi.Hal.Helpers;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the resource formatter entity. This must be inherited.
    /// </summary>
    public abstract class XmlResourceFormatter : IResourceFormatter
    {
        private bool _disposed;

        /// <summary>
        /// Initialises a new instance of the <see cref="XmlResourceFormatter" /> class.
        /// </summary>
        protected XmlResourceFormatter()
        {
            this.Settings = new XmlWriterSettings()
                            {
                                Indent = true,
                                OmitXmlDeclaration = false,
                                Encoding = Encoding.UTF8
                            };
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="XmlResourceFormatter" /> class.
        /// </summary>
        /// <param name="ns">The namespace for XML.</param>
        /// <param name="settings"><see cref="XmlWriterSettings" /> instance.</param>
        protected XmlResourceFormatter(string ns, XmlWriterSettings settings)
        {
            this.Namespace = ns;
            this.Settings = settings;
        }

        /// <summary>
        /// Gets the namespace for XML.
        /// </summary>
        protected string Namespace { get; }

        /// <summary>
        /// Gets the <see cref="XmlWriterSettings" /> value.
        /// </summary>
        protected XmlWriterSettings Settings { get; }

        /// <summary>
        /// Creates the <see cref="IResourceFormatter" /> instance.
        /// </summary>
        /// <param name="type"><see cref="Type" /> to check.</param>
        /// <param name="settings"><see cref="XmlWriterSettings" /> instance.</param>
        /// <returns>Returns the <see cref="IResourceFormatter" /> instance created.</returns>
        public static IResourceFormatter Create(Type type, string ns, XmlWriterSettings settings)
        {
            IResourceFormatter formatter;
            if (FormatterHelper.IsLinkedResourceCollectionType(type))
            {
                formatter = new XmlLinkedResourceCollectionFormatter(ns, settings);
            }
            else
            {
                formatter = new XmlLinkedResourceFormatter(ns, settings);
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
        /// Called during the <see cref="LinkedResource" /> serialisation.
        /// </summary>
        /// <param name="resource"><see cref="LinkedResource" /> instance.</param>
        /// <param name="objects">List of objects for serialisation.</param>
        public abstract void OnSerialisingResource(LinkedResource resource, params object[] objects);

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
        /// Writes the value to the output stream by serialising in XML.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="writeStream">The stream to write to.</param>
        /// <param name="resource"><see cref="LinkedResource" /> instance.</param>
        protected void WriteToStream(Type type, Stream writeStream, LinkedResource resource)
        {
            using (var writer = XmlWriter.Create(writeStream, this.Settings))
            {
                if (string.IsNullOrWhiteSpace(this.Namespace))
                {
                    writer.WriteStartElement("resource");
                }
                else
                {
                    writer.WriteStartElement("resource", this.Namespace);
                }

                this.SetSelfLink(resource);
                this.SerialiseLinks(writer, resource.Links);
                this.OnSerialisingResource(resource, type, writer);

                writer.WriteEndElement();
                writer.Flush();
            }
        }

        /// <summary>
        /// Sets the <see cref="Link" /> onto the given <see cref="LinkedResource" /> instance.
        /// </summary>
        /// <param name="resource"><see cref="LinkedResource" /> instance.</param>
        protected void SetSelfLink(LinkedResource resource)
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

        /// <summary>
        /// Serialises the list of <see cref="Link" /> objects.
        /// </summary>
        /// <param name="writer"><see cref="XmlWriter" /> instance.</param>
        /// <param name="links">List of <see cref="Link" /> objects to serialise.</param>
        protected void SerialiseLinks(XmlWriter writer, IEnumerable<Link> links)
        {
            if (!links.Any())
            {
                return;
            }

            writer.WriteStartElement("links");

            foreach (var link in links)
            {
                writer.WriteStartElement("link");

                writer.WriteElementString("rel", link.Rel);
                writer.WriteElementString("href", link.Href);

                if (link.ShouldSerializeIsHrefTemplated())
                {
                    writer.WriteElementString("templated", link.IsHrefTemplated.ToString().ToLowerInvariant());
                }

                this.SerialiseOptionalParameters(writer, link.OptionalParameters);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serialises the list of <see cref="OptionalParameter" /> objects.
        /// </summary>
        /// <param name="writer"><see cref="XmlWriter" /> instance.</param>
        /// <param name="parameters">List of <see cref="OptionalParameter" /> objects to serialise.</param>
        protected void SerialiseOptionalParameters(XmlWriter writer, IEnumerable<OptionalParameter> parameters)
        {
            if (!parameters.Any())
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                writer.WriteElementString(parameter.Key.ToString().ToLowerInvariant(), parameter.Value);
            }
        }

        /// <summary>
        /// Serialises properties on the <see cref="LinkedResource" /> class.
        /// </summary>
        /// <param name="writer"><see cref="XmlWriter" /> instance.</param>
        /// <param name="resource"><see cref="LinkedResource" /> instance.</param>
        protected void SerialiseProperties(XmlWriter writer, LinkedResource resource)
        {
            var properties = resource.GetType()
                                     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Where(p => !p.Name.Equals("rel", StringComparison.InvariantCultureIgnoreCase) &&
                                                 !p.Name.Equals("href", StringComparison.InvariantCultureIgnoreCase));

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

        /// <summary>
        /// Serialises the <see cref="LinkedResource" /> class.
        /// </summary>
        /// <param name="writer"><see cref="XmlWriter" /> instance.</param>
        /// <param name="resource"><see cref="LinkedResource" /> instance.</param>
        protected void SerialiseInnerResource(XmlWriter writer, LinkedResource resource)
        {
            writer.WriteStartElement("resource");

            this.SetSelfLink(resource);
            this.SerialiseLinks(writer, resource.Links);
            this.SerialiseProperties(writer, resource);

            writer.WriteEndElement();
        }
    }
}