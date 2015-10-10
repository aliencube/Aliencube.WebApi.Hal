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
    /// <summary>
    /// This represents the test entity for the <see cref="JsonLinkedResourceFormatter" /> class.
    /// </summary>
    [TestFixture]
    public class JsonLinkedResourceFormatterTest
    {
        private JsonSerializerSettings _settings;
        private IResourceFormatter _formatter;

        /// <summary>
        /// Initialises resources for the test class.
        /// </summary>
        [SetUp]
        public void Init()
        {
            this._settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            this._formatter = new JsonLinkedResourceFormatter(this._settings);
        }

        /// <summary>
        /// Releases all resources.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
        }

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

                var links = jo.SelectToken("_links");
                links.Should().NotBeNullOrEmpty();

                var self = links.SelectToken("self");
                self.Should().NotBeNullOrEmpty();

                var embedded = jo.SelectToken("_embedded");
                embedded.Should().BeNullOrEmpty();

                var href = jo.SelectToken("href");
                href.Should().BeNullOrEmpty();
            }
        }
    }
}