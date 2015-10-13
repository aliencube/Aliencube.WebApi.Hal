using System;
using System.Collections.Generic;

using Aliencube.WebApi.App.Resources;

namespace Aliencube.WebApi.App.Converters
{
    /// <summary>
    /// This represents the equality comparer entity for <see cref="Link" />.
    /// </summary>
    internal sealed class LinkEqualityComparer : IEqualityComparer<Link>
    {
        /// <summary>
        /// Check whether both given links are equal to each other or not.
        /// </summary>
        /// <param name="l1">First <see cref="Link" /> instance.</param>
        /// <param name="l2">Second <see cref="Link" /> instance. </param>
        /// <returns>
        /// Returns <c>True</c>, if both links are the same as each other; otherwise returns <c>False</c>.
        /// </returns>
        public bool Equals(Link l1, Link l2)
        {
            var result = string.Equals(l1.Rel, l2.Rel, StringComparison.InvariantCultureIgnoreCase) &&
                         string.Equals(l1.Href, l2.Href, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets hash code of the <see cref="Link" /> instance.
        /// </summary>
        /// <param name="link">The <see cref="Link" /> instance.</param>
        /// <returns>Returns the hash code generated.</returns>
        public int GetHashCode(Link link)
        {
            var norel = string.IsNullOrWhiteSpace(link.Rel) ? "norel" : link.Rel;
            var nohref = string.IsNullOrWhiteSpace(link.Href) ? "nohref" : link.Href;
            var joined = string.Join("~", norel, nohref);
            var hash = joined.GetHashCode();
            return hash;
        }
    }
}