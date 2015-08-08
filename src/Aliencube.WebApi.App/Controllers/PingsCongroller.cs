using System.Web.Http;

using Aliencube.WebApi.App.Models;
using Aliencube.WebApi.Hal.Extensions;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.App.Controllers
{
    [RoutePrefix("pings")]
    public class PingsCongroller : ApiController
    {
        [Route("message")]
        public Pings Get(string message)
        {
            var link = new Link() { Rel = "self", Href = string.Format("/ping/{0}", message) };
            var pings = new Pings
                        {
                            new Ping() { Message = string.Format("Hello, the 1st {0}", message) },
                            new Ping() { Message = string.Format("Hello, the 2nd {0}", message) }
                        };
            pings.AddLink(link);

            return pings;
        }
    }
}