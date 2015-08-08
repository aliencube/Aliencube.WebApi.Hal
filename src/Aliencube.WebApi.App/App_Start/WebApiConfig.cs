using System.Web.Http;

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
        /// <param name="builder"></param>
        /// <param name="container"></param>
        public static void Configure(IAppBuilder builder, IContainer container)
        {
            var config = new HttpConfiguration()
                         {
                             DependencyResolver = new AutofacWebApiDependencyResolver(container),
                         };

            // Routes
            config.MapHttpAttributeRoutes();

            // Formatters
            config.Formatters.Clear();
            config.Formatters.Add(new HalJsonMediaTypeFormatter()
                                  {
                                      SerializerSettings = new JsonSerializerSettings()
                                                           {
                                                               ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                               MissingMemberHandling = MissingMemberHandling.Ignore,
                                                           },
                                  });
            config.Formatters.Add(new HalXmlMediaTypeFormatter()
                                  {
                                      Namespace = "http://aliencube.org/schema/2015/08/sample"
                                  });

            builder.UseWebApi(config);
        }
    }
}