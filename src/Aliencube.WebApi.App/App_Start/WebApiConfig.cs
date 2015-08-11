using System;
using System.Web.Http;

using Aliencube.WebApi.Hal.Configs;
using Aliencube.WebApi.Hal.Formatters;

using Autofac;
using Autofac.Integration.WebApi;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Owin;

namespace Aliencube.WebApi.App
{
    /// <summary>
    /// This represents the config entity for Web API.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Configures the Web API.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="IAppBuilder" /> instance.
        /// </param>
        /// <param name="container">
        /// The <see cref="IContainer" /> instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws when either <c>builder</c> or <c>container</c> is null.
        /// </exception>
        public static void Configure(IAppBuilder builder, IContainer container)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var config = new HttpConfiguration()
                         {
                             DependencyResolver = new AutofacWebApiDependencyResolver(container),
                         };

            // Routes
            config.MapHttpAttributeRoutes();

            // Formatters
            config.ConfigHalFormatter();

            builder.UseWebApi(config);
        }
    }
}