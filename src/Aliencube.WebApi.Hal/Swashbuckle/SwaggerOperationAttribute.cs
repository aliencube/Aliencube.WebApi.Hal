using System;

namespace Aliencube.WebApi.Hal.Swashbuckle
{
    /// <summary>
    /// This represents the attribute entity for Swagger operation.
    /// </summary>
    /// <remarks>
    /// http://blog.greatrexpectations.com/2015/03/18/custom-operation-names-with-swashbuckle-5-0/
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SwaggerOperationAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SwaggerOperationAttribute" /> class.
        /// </summary>
        /// <param name="operationId"></param>
        public SwaggerOperationAttribute(string operationId)
        {
            this.OperationId = operationId;
        }

        /// <summary>
        /// Gets the operation Id.
        /// </summary>
        public string OperationId { get; }
    }
}