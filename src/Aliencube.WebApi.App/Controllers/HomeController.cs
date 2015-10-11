using System.Collections.Generic;
using System.Web.Http;

using Aliencube.WebApi.App.Models;
using Aliencube.WebApi.Hal.Extensions;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.App.Controllers
{
    /// <summary>
    /// This represents the controller entity for home.
    /// </summary>
    [RoutePrefix("")]
    public class HomeController : BaseApiController
    {
        /// <summary>
        /// Gets the <see cref="Index" /> instance.
        /// </summary>
        /// <returns>Returns the <see cref="Index" /> instance.</returns>
        [Route("")]
        public virtual Index Get()
        {
            var index = new Index();
            index.Href = this.Request.RequestUri.PathAndQuery;

            var links = new List<Link>()
                        {
                            new Link() { Rel = "collection", Href = "/products" },
                            new Link() { Rel = "template", Href = "/product/{productId}" },
                        };

            index.AddLinks(links);

            return index;
        }
    }
}