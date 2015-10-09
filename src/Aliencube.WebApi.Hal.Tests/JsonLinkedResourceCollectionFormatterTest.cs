using System.IO;
using System.Linq;
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
    /// This represents the test entity for the <see cref="JsonLinkedResourceCollectionFormatter" /> class.
    /// </summary>
    [TestFixture]
    public class JsonLinkedResourceCollectionFormatterTest
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
            this._formatter = new JsonLinkedResourceCollectionFormatter(this._settings);
        }

        /// <summary>
        /// Releases all resources.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
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

                var links = jo.SelectToken("_links");
                links.Should().NotBeNullOrEmpty();

                var self = links.SelectToken("self");
                self.Should().NotBeNullOrEmpty();

                var embedded = jo.SelectToken("_embedded");
                embedded.Should().NotBeNullOrEmpty();
                embedded.Count().Should().Be(2);

                var href = jo.SelectToken("href");
                href.Should().BeNullOrEmpty();
            }
        }
    }
}