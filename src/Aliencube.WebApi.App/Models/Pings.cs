using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.App.Models
{
    /// <summary>
    /// This represents the model entity for the list of pings.
    /// </summary>
    public class Pings : LinkedResourceCollection<Ping>
    {
    }
}