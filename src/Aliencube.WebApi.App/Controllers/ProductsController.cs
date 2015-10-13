using System.Web.Http;

using Aliencube.WebApi.App.Helpers;
using Aliencube.WebApi.App.Models;
using Aliencube.WebApi.App.Resources;

namespace Aliencube.WebApi.App.Controllers
{
    /// <summary>
    /// This represents the controller entity for product collection.
    /// </summary>
    [RoutePrefix("products")]
    public class ProductsController : BaseApiController
    {
        /// <summary>
        /// Gets the <see cref="Products" /> instance.
        /// </summary>
        /// <returns>
        /// Returns the <see cref="Products" /> instance.
        /// </returns>
        [Route("")]
        public virtual Products Get()
        {
            var products = ProductHelper.GetProducts();

            products.Href = this.Request.RequestUri.PathAndQuery;
            products.Links.Add(new Link() { Rel = "next", Href = "/products?p=2" });
            products.Links.Add(new Link() { Rel = "template", Href = "/product/{productId}" });

            return products;
        }
    }
}