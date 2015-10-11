using System.Linq;
using System.Web.Http.Description;

using Aliencube.WebApi.Hal.Helpers;

using Swashbuckle.Swagger;

namespace Aliencube.WebApi.Hal.Swashbuckle
{
    /// <summary>
    /// This represents the filter entity for Swagger operation.
    /// </summary>
    /// <remarks>
    /// http://blog.greatrexpectations.com/2015/03/18/custom-operation-names-with-swashbuckle-5-0/
    /// </remarks>
    public class SwaggerOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the Swagger operation filter.
        /// </summary>
        /// <param name="operation"><see cref="Operation" /> instance.</param>
        /// <param name="schemaRegistry"><see cref="SchemaRegistry" /> instance.</param>
        /// <param name="apiDescription"><see cref="ApiDescription" /> instance.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var type = apiDescription.ActionDescriptor.ReturnType;
            if (FormatterHelper.IsLinkedResourceCollectionType(type))
            {
                operation.responses.First().Value.schema.additionalProperties = new Schema() { type = "object" };
            }

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