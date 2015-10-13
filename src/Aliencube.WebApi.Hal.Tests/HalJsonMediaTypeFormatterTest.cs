using System.IO;
using System.Linq;
using System.Text;

using Aliencube.WebApi.App.Formatters;
using Aliencube.WebApi.App.Tests.Helpers;
using Aliencube.WebApi.App.Tests.Models;

using FluentAssertions;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using NUnit.Framework;

namespace Aliencube.WebApi.App.Tests
{
    /// <summary>
    /// This represents the test entity for the <see cref="HalJsonMediaTypeFormatter" /> class.
    /// </summary>
    [TestFixture]
    public class HalJsonMediaTypeFormatterTest
    {
        private HalJsonMediaTypeFormatter _formatter;

        /// <summary>
        /// Initialises resources for the test class.
        /// </summary>
        [SetUp]
        public void Init()
        {
            this._formatter = new HalJsonMediaTypeFormatter()
                              {
                                  SerializerSettings =
                                  {
                                      ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                      MissingMemberHandling = MissingMemberHandling.Ignore,
                                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                      NullValueHandling = NullValueHandling.Ignore,
                                  }
                              };
        }

        /// <summary>
        /// Releases all resources.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
        }

        ///// <summary>
        ///// Tests whether the given type can be read or not.
        ///// </summary>
        //[Test]
        //public void GivenTypeShouldBeAbleToRead()
        //{
        //    var product = ProductHelper.GetProduct(1);

        //    var result = this._formatter.CanReadType(product.GetType());
        //    result.Should().BeTrue();

        //    var products = ProductHelper.GetProducts(2);

        //    result = this._formatter.CanReadType(products.GetType());
        //    result.Should().BeTrue();
        //}

        ///// <summary>
        ///// Tests whether the given type can be written or not.
        ///// </summary>
        //[Test]
        //public void GivenTypeShouldBeAbleToWrite()
        //{
        //    var product = ProductHelper.GetProduct(1);

        //    var result = this._formatter.CanWriteType(product.GetType());
        //    result.Should().BeTrue();

        //    var products = ProductHelper.GetProducts(2);

        //    result = this._formatter.CanWriteType(products.GetType());
        //    result.Should().BeTrue();
        //}

        /// <summary>
        /// Tests whether the given product returns JSON objects formatted in HAL.
        /// </summary>
        [Test]
        public void GivenProductShouldReturnHalJsonObject()
        {
            var product = ProductHelper.GetProduct(1);

            using (var stream = new MemoryStream())
            {
                this._formatter.WriteToStream(typeof(Product), product, stream, Encoding.UTF8);
                var jo = FormatterHelper.ParseJsonStream(stream);

                var rel = jo.SelectToken("rel");
                rel.Should().BeNullOrEmpty();

                var href = jo.SelectToken("href");
                href.Should().BeNullOrEmpty();

                var links = jo.SelectToken("_links");
                links.Should().NotBeNullOrEmpty();

                var embedded = jo.SelectToken("_embedded");
                embedded.Should().BeNullOrEmpty();
            }
        }

        /// <summary>
        /// Tests whether the given products returns JSON objects formatted in HAL.
        /// </summary>
        [Test]
        public void GivenProductsShouldReturnHalJsonObject()
        {
            var products = ProductHelper.GetProducts(2);

            using (var stream = new MemoryStream())
            {
                this._formatter.WriteToStream(typeof(Products), products, stream, Encoding.UTF8);
                var jo = FormatterHelper.ParseJsonStream(stream);

                var rel = jo.SelectToken("rel");
                rel.Should().BeNullOrEmpty();

                var href = jo.SelectToken("href");
                href.Should().BeNullOrEmpty();

                var links = jo.SelectToken("_links");
                links.Should().NotBeNullOrEmpty();

                var embedded = jo.SelectToken("_embedded");
                embedded.Should().NotBeNullOrEmpty();
                embedded.Count().Should().Be(2);
            }
        }
    }
}