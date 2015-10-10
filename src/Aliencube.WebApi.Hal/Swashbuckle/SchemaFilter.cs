using System;
using System.Linq;
using System.Web.Http.Description;
using Aliencube.WebApi.Hal.Helpers;

using Swashbuckle.Swagger;
using Swashbuckle.Swagger.Annotations;

namespace Aliencube.WebApi.Hal.Swashbuckle
{
    /// <summary>
    /// This represents the filter entity for HAL schema.
    /// </summary>
    public class HalSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies schema filter.
        /// </summary>
        /// <param name="schema"><see cref="Schema" /> instance.</param>
        /// <param name="schemaRegistry"><see cref="SchemaRegistry" /> instance.</param>
        /// <param name="type"><see cref="Type" /> instance.</param>
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (FormatterHelper.IsLinkedResourceType(type))
            {
                schema.type = "object";
                schema.properties.Clear();
                schema.additionalProperties = new Schema() { type = "object" };
                schema.example = new { self = new { rel = "self", href = "uri" } };
            }

            if (FormatterHelper.IsLinkedResourceCollectionType(type))
            {
                schema.type = "object";
                schema.properties.Clear();
                schema.additionalProperties = new Schema() { type = "object" };
                schema.example = new { item = new { }, item2 = new { } };
            }
        }
    }

    /// <summary>
    /// This represents the filter entity for operation name.
    /// </summary>
    /// <remarks>
    /// http://blog.greatrexpectations.com/2015/03/18/custom-operation-names-with-swashbuckle-5-0/
    /// </remarks>
    public class OperationNameFilter : IOperationFilter
    {
        /// <summary>
        /// Applies operation name filter.
        /// </summary>
        /// <param name="operation"><see cref="Operation" /> instance.</param>
        /// <param name="schemaRegistry"><see cref="SchemaRegistry" /> instance.</param>
        /// <param name="apiDescription"><see cref="ApiDescription" /> instance.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var overwriteOperationId = apiDescription.ActionDescriptor
                                                     .GetCustomAttributes<SwaggerOperationAttribute>()
                                                     .Select(a => a.OperationId)
                                                     .FirstOrDefault();

            if (overwriteOperationId == null)
            {
                return;
            }

            operation.operationId = overwriteOperationId;
        }
    }
}