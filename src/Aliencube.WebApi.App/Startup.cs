using System;
using System.Web.Http;

using Autofac.Integration.WebApi;

using Owin;

namespace Aliencube.WebApi.App
{
    /// <summary>
    /// This represents the entity for OWIN pipeline startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the OWIN pipeline.
        /// </summary>
        /// <param name="appBuilder">The app builder</param>
        public void Configuration(IAppBuilder appBuilder)
        {
            if (appBuilder == null)
            {
                throw new ArgumentNullException("appBuilder");
            }

            var container = DependencyConfig.Configure();

            WebApiConfig.Configure(appBuilder, container);
        }
    }
}