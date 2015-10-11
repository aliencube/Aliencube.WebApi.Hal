using System;

using Aliencube.WebApi.Hal.Helpers;

using Swashbuckle.Swagger;

namespace Aliencube.WebApi.Hal.Swashbuckle
{
    /// <summary>
    /// This represents the filter entity for HAL schema.
    /// </summary>
    public class SwaggerHalSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies schema filter.
        /// </summary>
        /// <param name="schema"><see cref="Schema" /> instance.</param>
        /// <param name="schemaRegistry"><see cref="SchemaRegistry" /> instance.</param>
        /// <param name="type"><see cref="Type" /> instance.</param>
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (FormatterHelper.IsLinkedResourceCollectionType(type))
            {
                schema.type = "object";
                //schema.properties.Clear();
                schema.additionalProperties = new Schema() { type = "object" };
                schema.example = new { item = new { }, item2 = new { } };
            }
            else if (FormatterHelper.IsLinkedResourceType(type))
            {
                schema.type = "object";
                //schema.properties.Clear();
                schema.additionalProperties = new Schema() { type = "object" };
                schema.example = new { self = new { rel = "self", href = "uri" } };
            }
        }
    }
}