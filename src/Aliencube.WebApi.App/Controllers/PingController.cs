using System;
using System.Collections.Generic;
using System.Web.Http;

using Aliencube.WebApi.App.Models;
using Aliencube.WebApi.Hal.Extensions;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.App.Controllers
{
    /// <summary>
    /// This represents the controller entity for ping.
    /// </summary>
    [RoutePrefix("ping")]
    public class PingController : ApiController
    {
        /// <summary>
        /// Gets the <see cref="Ping" /> object containing message.
        /// </summary>
        /// <param name="name">
        /// Message value.
        /// </param>
        /// <returns>
        /// Returns the <see cref="Ping" /> object containing message.
        /// </returns>
        [Route("{name}")]
        public Ping Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            var ping = new Ping() { Message = string.Format("Hello, {0}", name) };
            ping.AddLinks(new List<Link>()
                              {
                                  new Link() { Rel = "self", Href = string.Format("/ping/{0}", name) },
                                  new Link() { Rel = "rel", Href = "/pings/{message}" }
                              });

            return ping;
        }
    }
}