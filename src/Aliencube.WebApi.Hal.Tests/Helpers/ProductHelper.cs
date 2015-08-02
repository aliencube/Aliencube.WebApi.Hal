using System.Collections.Generic;
using Aliencube.WebApi.Hal.Resources;
using Aliencube.WebApi.Hal.Tests.Models;

namespace Aliencube.WebApi.Hal.Tests.Helpers
{
    /// <summary>
    /// This represents the helper entity for product.
    /// </summary>
    public static class ProductHelper
    {
        /// <summary>
        /// Gets the <see cref="Product" /> object.
        /// </summary>
        /// <param name="productId">Product Id.</param>
        /// <returns>Returns the <see cref="Product" /> object.</returns>
        public static Product GetProduct(int productId)
        {
            var product = new Product()
                          {
                              ProductId = productId,
                              Name = string.Format("Product{0}", productId),
                              Description = string.Format("Product Description {0}", productId),
                              Rel = "self",
                              Href = string.Format("/products/{0}", productId),
                          };

            product.Links.Add(new Link() { Rel = "self", Href = string.Format("/products/{0}", productId) });
            product.Links.Add(new Link() { Rel = "rel", Href = "/products" });

            return product;
        }

        /// <summary>
        /// Gets the given number of products.
        /// </summary>
        /// <param name="count">Number of items to set.</param>
        /// <returns>Returns the given number of products.</returns>
        public static Products GetProducts(int count)
        {
            var items = new List<Product>();
            for (var i = 0; i < count; i++)
            {
                var item = GetProduct(i + 1);
                items.Add(item);
            }

            var products = new Products(items);
            products.Links.Add(new Link() { Rel = "self", Href = "/products" });
            return products;
        }
    }
}
