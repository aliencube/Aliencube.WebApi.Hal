﻿using System.Collections.Generic;

using Newtonsoft.Json;

namespace Aliencube.WebApi.Hal.Resources
{
    /// <summary>
    /// This represents the entity for resources containing links. This must be inherited.
    /// </summary>
    public abstract class LinkedResource
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LinkedResource" /> class.
        /// </summary>
        protected LinkedResource()
        {
            this.Links = new List<Link>();
        }

        /// <summary>
        /// Gets the list of <see cref="Link" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public List<Link> Links { get; }

        /// <summary>
        /// Gets the list of <see cref="LinkedResource" /> objects.
        /// </summary>
        [JsonProperty(PropertyName = "_embedded")]
        public List<LinkedResource> Embedded { get; }
    }
}