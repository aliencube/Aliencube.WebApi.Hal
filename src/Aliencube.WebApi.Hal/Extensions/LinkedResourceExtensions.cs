using System;
using System.Collections.Generic;
using System.Linq;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Extensions
{
    /// <summary>
    /// This represents the extension entity for the <see cref="LinkedResource" /> class.
    /// </summary>
    public static class LinkedResourceExtensions
    {
        /// <summary>
        /// Adds a <see cref="Link" /> object to the <see cref="LinkedResource" /> object.
        /// </summary>
        /// <param name="resource">Current <see cref="LinkedResource" /> object.</param>
        /// <param name="link"><see cref="Link" /> object to add.</param>
        /// <returns>Returns the <see cref="LinkedResource" /> object containing the <see cref="Link" /> object.</returns>
        public static LinkedResource AddLink(this LinkedResource resource, Link link)
        {
            if (link == null)
            {
                return resource;
            }

            resource.Links.Add(link);
            return resource;
        }

        /// <summary>
        /// Adds a list of <see cref="Link" /> object to the <see cref="LinkedResource" /> object.
        /// </summary>
        /// <param name="resource">Current <see cref="LinkedResource" /> object.</param>
        /// <param name="links">List of <see cref="Link" /> objects to add.</param>
        /// <returns>Returns the <see cref="LinkedResource" /> object containing the list of <see cref="Link" /> objects.</returns>
        public static LinkedResource AddLinks(this LinkedResource resource, IList<Link> links)
        {
            if (links == null || !links.Any())
            {
                return resource;
            }

            resource.Links.AddRange(links);
            return resource;
        }
    }
}