using System.Collections.Generic;

using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Tests.Models
{
    /// <summary>
    /// This represents the entity for product set.
    /// </summary>
    public class Products : LinkedResource
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Products" /> class.
        /// </summary>
        public Products()
            : base()
        {
            this.Embedded = new LinkedResourceCollection();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Products" /> class.
        /// </summary>
        /// <param name="items">List of <see cref="Product" /> objects.</param>
        public Products(List<Product> items)
            : this()
        {
            this.Embedded.AddRange(items);
        }
    }
}