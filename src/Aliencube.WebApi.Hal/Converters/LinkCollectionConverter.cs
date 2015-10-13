using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Aliencube.WebApi.Hal.Resources;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Converters
{
    /// <summary>
    /// This represents the converter entity for the <see cref="LinkCollection" /> class.
    /// </summary>
    public class LinkCollectionConverter : JsonConverter
    {
        private static readonly IEqualityComparer<Link> ComparerInstance = new LinkEqualityComparer();

        /// <summary>
        /// Gets the <see cref="LinkEqualityComparer" /> instance.
        /// </summary>
        public static IEqualityComparer<Link> EqualityComparer
        {
            get { return ComparerInstance; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JsonConverter" /> can read JSON.
        /// </summary>
        public override bool CanRead
        {
            get { return false; }
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// Returns <c>True</c>, if this instance can convert the specified object type; otherwise, returns <c>False</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            var result = typeof(IList<Link>).IsAssignableFrom(objectType) ||
                         typeof(LinkCollection).IsAssignableFrom(objectType);
            return result;
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serialiser">The calling serialiser.</param>
        /// <returns>
        /// Returns the object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serialiser)
        {
            return reader.Value;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serialiser">The calling serialiser.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serialiser)
        {
            var collection = new List<Link>();
            if (value is LinkCollection)
            {
                collection = ((LinkCollection)value).Items;
            }

            var links = new HashSet<Link>(collection, EqualityComparer);
            var lookup = links.ToLookup(p => p.Rel);

            if (lookup.Count == 0)
            {
                return;
            }

            writer.WriteStartObject();

            foreach (var rel in lookup)
            {
                var count = rel.Count();

                writer.WritePropertyName(rel.Key);

                if (count > 1)
                {
                    writer.WriteStartArray();
                }

                foreach (var link in rel)
                {
                    WriteLink(writer, link);
                }

                if (count > 1)
                {
                    writer.WriteEndArray();
                }
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Resolves href value into URI.
        /// </summary>
        /// <param name="href">Href value.</param>
        /// <returns>Returns the resolved URI.</returns>
        public virtual string ResolveUri(string href)
        {
            if (!string.IsNullOrEmpty(href) && VirtualPathUtility.IsAppRelative(href))
            {
                return HttpContext.Current != null ? VirtualPathUtility.ToAbsolute(href) : href.Replace("~/", "/");
            }

            return href;
        }

        private void WriteLink(JsonWriter writer, Link link)
        {
            writer.WriteStartObject();

            foreach (var info in link.GetType().GetProperties())
            {
                switch (info.Name)
                {
                    case "Rel":
                        // do nothing ...
                        break;

                    case "Href":
                        writer.WritePropertyName("href");
                        writer.WriteValue(this.ResolveUri(link.Href));
                        break;

                    case "IsHrefTemplated":
                        if (link.IsHrefTemplated)
                        {
                            writer.WritePropertyName("templated");
                            writer.WriteValue(true);
                        }

                        break;

                    default:
                        if (info.PropertyType == typeof(string))
                        {
                            var text = info.GetValue(link) as string;

                            if (string.IsNullOrEmpty(text))
                            {
                                continue; // no value set, so don't write this property ...
                            }

                            writer.WritePropertyName(info.Name.ToLowerInvariant());
                            writer.WriteValue(text);
                        }

                        // else: no sensible way to serialize ...
                        break;
                }
            }

            writer.WriteEndObject();
        }
    }
}