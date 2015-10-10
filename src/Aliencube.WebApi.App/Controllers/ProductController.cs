﻿using System.Web.Http;

using Aliencube.WebApi.App.Helpers;
using Aliencube.WebApi.App.Models;

namespace Aliencube.WebApi.App.Controllers
{
    /// <summary>
    /// This represents the controller entity for product.
    /// </summary>
    [RoutePrefix("product")]
    public class ProductController : BaseApiController
    {
        /// <summary>
        /// Gets the <see cref="Product" /> instance.
        /// </summary>
        /// <param name="productId">
        /// The product id.
        /// </param>
        /// <returns>
        /// Returns the <see cref="Product" /> instance.
        /// </returns>
        [Route("{productId}")]
        public virtual Product Get(int productId)
        {
            var product = ProductHelper.GetProduct(productId);

            product.Href = this.Request.RequestUri.PathAndQuery;

            return product;
        }
    }
}