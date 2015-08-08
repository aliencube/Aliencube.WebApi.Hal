using System.Web.Http;

using Aliencube.WebApi.App.Models;
using Aliencube.WebApi.Hal.Extensions;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.App.Controllers
{
    [RoutePrefix("ping")]
    public class PingController : ApiController
    {
        [Route("message")]
        public Ping Get(string message)
        {
            var link = new Link() { Rel = "self", Href = string.Format("/ping/{0}", message) };
            var ping = new Ping() { Message = string.Format("Hello, {0}", message) };
            ping.AddLink(link);

            return ping;
        }
    }
}