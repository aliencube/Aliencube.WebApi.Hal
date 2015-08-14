using System.Collections.Generic;
using System.Linq;

using Aliencube.WebApi.App.Models;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.App.Helpers
{
    /// <summary>
    /// This represents the helper entity for products.
    /// </summary>
    public static class ProductHelper
    {
        /// <summary>
        /// Gets the list of products.
        /// </summary>
        /// <returns>
        /// Returns the list of products.
        /// </returns>
        public static Products GetProducts()
        {
            var products =
                new Products(
                    new List<Product>()
                        {
                            new Product()
                                {
                                    ProductId = 1,
                                    Name = "ABC",
                                    Description = "Product ABC",
                                    Href = "/product/1",
                                    Links =
                                        {
                                            new Link() { Rel = "collection", Href = "/products" },
                                            new Link() { Rel = "template", Href = "/product/{productId}" },
                                        }
                                },
                            new Product()
                                {
                                    ProductId = 2,
                                    Name = "XYZ",
                                    Description = "Product XYZ",
                                    Href = "/product/2",
                                    Links =
                                        {
                                            new Link() { Rel = "collection", Href = "/products" },
                                            new Link() { Rel = "template", Href = "/product/{productId}" },
                                        }
                                },
                        });
            return products;
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">
        /// The product id.
        /// </param>
        /// <returns>
        /// Returns the product.
        /// </returns>
        public static Product GetProduct(int productId)
        {
            var product = GetProducts().SingleOrDefault(p => p.ProductId == productId);
            return product;
        }
    }
}