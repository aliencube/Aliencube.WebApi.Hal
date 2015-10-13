using System.Collections.Generic;

using Aliencube.WebApi.App.Resources;
using Aliencube.WebApi.App.Tests.Models;

namespace Aliencube.WebApi.App.Tests.Helpers
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
                              Href = string.Format("/products/{0}", productId),
                          };

            var links = new List<Link>()
                        {
                            new Link() { Rel = "find", Href = "/products{?id}" },
                            new Link() { Rel = "rel", Href = "/products" },
                            new Link() { Rel = "optional", Href = "/products/optional", Title = "Sample Title" },
                        };
            product.Links.AddRange(links);

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

            var link = new Link() { Rel = "self", Href = "/products" };
            products.Links.Add(link);

            return products;
        }
    }
}