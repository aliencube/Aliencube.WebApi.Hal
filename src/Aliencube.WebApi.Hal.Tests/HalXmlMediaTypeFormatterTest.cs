using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Aliencube.WebApi.Hal.Extensions;
using Aliencube.WebApi.Hal.Formatters;
using Aliencube.WebApi.Hal.Tests.Helpers;
using Aliencube.WebApi.Hal.Tests.Models;

using FluentAssertions;

using NUnit.Framework;

namespace Aliencube.WebApi.Hal.Tests
{
    /// <summary>
    /// This represents the test entity for the <see cref="HalXmlMediaTypeFormatter" /> class.
    /// </summary>
    [TestFixture]
    public class HalXmlMediaTypeFormatterTest
    {
        private HalXmlMediaTypeFormatter _formatter;

        /// <summary>
        /// Initialises resources for the test class.
        /// </summary>
        [SetUp]
        public void Init()
        {
            this._formatter = new HalXmlMediaTypeFormatter()
                              {
                                  Namespace = "http://test.com/halxmlmediatypeformatter"
                              };
        }

        /// <summary>
        /// Releases all resources.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
        }

        /// <summary>
        /// Tests whether the given type can be read or not.
        /// </summary>
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

        /// <summary>
        /// Tests whether the given type can be written or not.
        /// </summary>
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

        /// <summary>
        /// Tests whether the given product returns XML objects formatted in HAL.
        /// </summary>
        [Test]
        public void GivenProductShouldReturnHalXmlObject()
        {
            var product = ProductHelper.GetProduct(1);

            XDocument xml;
            using (var stream = new MemoryStream())
            {
                this._formatter.WriteToStream(typeof(Product), product, stream, null);
                xml = FormatterHelper.ParseXmlStream(stream);
            }

            var root = xml.Root;
            root.Should().NotBeNull();
            root.HasElements.Should().BeTrue();

            var links = root.Elements().SingleOrDefault(p => p.Name.LocalName.Equals("links", StringComparison.InvariantCultureIgnoreCase));
            links.Should().NotBeNull();
            links.Elements().Count().Should().BeGreaterOrEqualTo(1);

            var names = product.GetType()
                               .GetProperties()
                               .Where(pi => pi.Name != "Links" && pi.Name != "Rel" && pi.Name != "Href")
                               .Select(pi => pi.Name.ToCamelCase());
            foreach (var name in names)
            {
                var element = root.Elements()
                                      .SingleOrDefault(p => p.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                element.Should().NotBeNull();
            }
        }
    }
}