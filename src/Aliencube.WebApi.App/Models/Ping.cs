using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.App.Models
{
    /// <summary>
    /// This represents the model entity for ping.
    /// </summary>
    public class Ping : LinkedResource
    {
        /// <summary>
        /// Gets or sets the ping message.
        /// </summary>
        public string Message { get; set; }
    }
}