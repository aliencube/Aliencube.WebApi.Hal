using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Aliencube.WebApi.Hal.Extensions;
using Aliencube.WebApi.Hal.Formatters;
using Aliencube.WebApi.Hal.Tests.Helpers;
using Aliencube.WebApi.Hal.Tests.Models;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Aliencube.WebApi.Hal.Tests
{
    [TestFixture]
    public class XmlLinkedResourceFormatterTest
    {
        private const string Namespace = "http://test.com/halxmlmediatypeformatter";

        private Mock<HttpContent> _content;
        private XmlWriterSettings _settings;
        private IResourceFormatter _formatter;

        [SetUp]
        public void Init()
        {
            this._content = new Mock<HttpContent>();
            this._settings = new XmlWriterSettings()
                             {
                                 Indent = true,
                                 OmitXmlDeclaration = false,
                                 Encoding = Encoding.UTF8
                             };

            this._formatter = new XmlLinkedResourceFormatter(Namespace, this._settings);
        }

        [TearDown]
        public void Cleanup()
        {
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
                this._formatter.WriteToStream(typeof(Product), product, stream, this._content.Object);
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