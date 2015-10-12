﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aliencube.WebApi.Hal.Resources;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Converters
{
    /// <summary>
    /// This represents the converter entity for the <see cref="HalResourceCollection" /> class.
    /// </summary>
    public class HalResourceCollectionConverter : JsonConverter
    {
        private static readonly IEqualityComparer<HalResource> ComparerInstance = new HalResourceEqualityComparer();

        /// <summary>
        /// Gets the <see cref="LinkEqualityComparer" /> instance.
        /// </summary>
        public static IEqualityComparer<HalResource> EqualityComparer
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
            var result = typeof(IList<HalResource>).IsAssignableFrom(objectType) ||
                         typeof(HalResourceCollection).IsAssignableFrom(objectType);
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
            var collection = new List<HalResource>();
            if (value is HalResourceCollection)
            {
                collection = ((HalResourceCollection)value).Items;
            }

            var resources = new HashSet<HalResource>(collection, EqualityComparer);
            var lookup = resources.ToLookup(p => p.Rel);

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

                foreach (var er in rel)
                {
                    serialiser.Serialize(writer, er);
                }

                if (count > 1)
                {
                    writer.WriteEndArray();
                }
            }

            writer.WriteEndObject();
        }
    }
}
