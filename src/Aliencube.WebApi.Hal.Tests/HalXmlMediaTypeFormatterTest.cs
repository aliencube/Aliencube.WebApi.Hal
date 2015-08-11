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
    [TestFixture]
    public class HalXmlMediaTypeFormatterTest
    {
        private HalXmlMediaTypeFormatter _formatter;

        [SetUp]
        public void Init()
        {
            this._formatter = new HalXmlMediaTypeFormatter()
                              {
                                  Namespace = "http://test.com/halxmlmediatypeformatter"
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

            var resource = root.Elements()
                               .SingleOrDefault(p => p.Name.LocalName.Equals("resource", StringComparison.InvariantCultureIgnoreCase));
            var names = product.GetType()
                               .GetProperties()
                               .Where(pi => pi.Name != "Links" && pi.Name != "Rel" && pi.Name != "Href")
                               .Select(pi => pi.Name.ToCamelCase());
            foreach (var name in names)
            {
                var element = resource.Elements()
                                      .SingleOrDefault(p => p.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                element.Should().NotBeNull();
            }
        }
    }
}