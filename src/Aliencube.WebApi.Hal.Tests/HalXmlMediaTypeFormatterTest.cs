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

            root.Attribute("rel").Value.Should().Be(product.Rel);
            root.Attribute("href").Value.Should().Be(product.Href);

            var attrs = root.Elements()
                            .Where(p => !p.Name.LocalName.Equals("resource", StringComparison.InvariantCultureIgnoreCase))
                            .Select(p => new { rel = p.Attribute("rel").Value, href = p.Attribute("href").Value });
            var links = product.Links
                               .Where(p => !p.Rel.Equals("self", StringComparison.InvariantCultureIgnoreCase))
                               .Select(p => new { rel = p.Rel.ToCamelCase(), href = p.Href });
            links.Should().BeEquivalentTo(attrs);

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

        [Test]
        public void GivenProductsShouldReturnHalXmlObject()
        {
            var products = ProductHelper.GetProducts(2);

            XDocument xml;
            using (var stream = new MemoryStream())
            {
                this._formatter.WriteToStream(typeof(Products), products, stream, null);
                xml = FormatterHelper.ParseXmlStream(stream);
            }

            var root = xml.Root;
            root.Should().NotBeNull();

            root.Attribute("rel").Value.Should().Be(products.Rel);
            root.Attribute("href").Value.Should().Be(products.Href);

            var attrs = root.Elements()
                            .Where(p => !p.Name.LocalName.Equals("resource", StringComparison.InvariantCultureIgnoreCase))
                            .Select(p => new { rel = p.Attribute("rel").Value, href = p.Attribute("href").Value });
            var links = products.Links
                               .Where(p => !p.Rel.Equals("self", StringComparison.InvariantCultureIgnoreCase))
                               .Select(p => new { rel = p.Rel.ToCamelCase(), href = p.Href });
            links.Should().BeEquivalentTo(attrs);

            var resources = root.Elements().Where(p => p.Name.LocalName.Equals("resource", StringComparison.InvariantCultureIgnoreCase));
            resources.Count().Should().Be(products.Count);
        }
    }
}