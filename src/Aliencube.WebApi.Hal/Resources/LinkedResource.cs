using System;
using System.Linq;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for HAL resource. This must be inherited.
    /// </summary>
    public abstract class LinkedResource
    {
        private const string SelfRel = "self";

        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResource" /> class.
        /// </summary>
        protected LinkedResource()
        {
            this.Links = new LinkCollection();
        }

        /// <summary>
        /// Gets the relation of the <see cref="LinkedResource" />.
        /// </summary>
        [JsonIgnore]
        public string Rel
        {
            get { return SelfRel; }
        }

        /// <summary>
        /// Gets the href value of the <see cref="LinkedResource" />.
        /// </summary>
        [JsonIgnore]
        public string Href
        {
            get { return this.GetSelfHref(); }
            set { this.SetSelfHref(value); }
        }

        /// <summary>
        /// Gets the <see cref="LinkCollection" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public LinkCollection Links { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="LinkedResourceCollection" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_embedded")]
        public LinkedResourceCollection Embedded { get; set; }

        private string GetSelfHref()
        {
            if (!this.Links.Items.Any())
            {
                return null;
            }

            var self = this.Links
                           .Items
                           .SingleOrDefault(p => p.Rel.Equals(SelfRel, StringComparison.InvariantCultureIgnoreCase));
            if (self == null)
            {
                return null;
            }

            return self.Href;
        }

        private void SetSelfHref(string value)
        {
            var self = this.Links
                           .Items
                           .SingleOrDefault(p => p.Rel.Equals(SelfRel, StringComparison.InvariantCultureIgnoreCase));
            if (self != null)
            {
                self.Href = value;
                return;
            }

            self = new Link() { Rel = SelfRel, Href = value };
            this.Links.Add(self);
        }
    }
}