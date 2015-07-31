using System.IO;
using System.Text;
using Aliencube.WebApi.Hal.Formatters;
using Aliencube.WebApi.Hal.Resources;
using Aliencube.WebApi.Hal.Tests.Helpers;
using Aliencube.WebApi.Hal.Tests.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace Aliencube.WebApi.Hal.Tests
{
    [TestFixture]
    public class HalJsonMediaTypeFormatterTest
    {
        private HalJsonMediaTypeFormatter _formatter;

        [SetUp]
        public void Init()
        {
            this._formatter = new HalJsonMediaTypeFormatter()
                              {
                                  SerializerSettings =
                                  {
                                      ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                  }
                              };
        }

        [TearDown]
        public void Cleanup()
        {
        }

        [Test]
        public void GivenProductShouldReturnHalJsonObject()
        {
            var product = new Product()
                          {
                              ProductId = 1,
                              Name = "Product 1",
                              Description = "Product Description 1",
                              Rel = "self",
                              Href = "/products/1",
                          };
            product.Links.Add(new Link() { Rel = "self", Href = "/products/1" });
            product.Links.Add(new Link() { Rel = "rel", Href = "/products" });

            using (var stream = new MemoryStream())
            {
                this._formatter.WriteToStream(typeof(Product), product, stream, Encoding.UTF8);
                var jo = FormatterHelper.ParseJsonStream(stream);

                var links = jo.SelectToken("_links");
                links.Should().NotBeNullOrEmpty();

                var rel = jo.SelectToken("rel");
                rel.Should().BeNullOrEmpty();

                var href = jo.SelectToken("href");
                href.Should().BeNullOrEmpty();
            }
        }
    }
}