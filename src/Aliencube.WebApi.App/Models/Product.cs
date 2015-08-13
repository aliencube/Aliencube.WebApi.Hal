using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.App.Models
{
    /// <summary>
    /// This represents the entity for product.
    /// </summary>
    public class Product : LinkedResource
    {
        /// <summary>
        /// Gets or sets the product Id.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        public string Description { get; set; }
    }
}