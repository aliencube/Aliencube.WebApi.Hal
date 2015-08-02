using System.IO;
using System.Text;
using Aliencube.WebApi.Hal.Formatters;
using Aliencube.WebApi.Hal.Tests.Helpers;
using Aliencube.WebApi.Hal.Tests.Models;
using FluentAssertions;
using Newtonsoft.Json;
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
        public void GivenTypeShouldBeAbleToRead()
        {
            var product = ProductHelper.GetProduct(1);

            var result = this._formatter.CanReadType(product.GetType());
            result.Should().BeTrue();

            var products = ProductHelper.GetProducts(2);

            result = this._formatter.CanReadType(products.GetType());
            result.Should().BeTrue();
        }

        [Test]
        public void GivenTypeShouldBeAbleToWrite()
        {
            var product = ProductHelper.GetProduct(1);

            var result = this._formatter.CanWriteType(product.GetType());
            result.Should().BeTrue();

            var products = ProductHelper.GetProducts(2);

            result = this._formatter.CanWriteType(products.GetType());
            result.Should().BeTrue();
        }

        [Test]
        public void GivenProductShouldReturnHalJsonObject()
        {
            var product = ProductHelper.GetProduct(1);

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