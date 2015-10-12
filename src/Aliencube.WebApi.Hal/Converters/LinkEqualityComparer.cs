using System;
using System.Collections.Generic;

using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Converters
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
        /// <param name="l2">Secod <see cref="Link" /> instance. </param>
        /// <returns>
        /// Returns <c>True</c>, if both links are the same as each other; oterwise returns <c>False</c>.
        /// </returns>
        public bool Equals(Link l1, Link l2)
        {
            var result = string.Compare(l1.Href, l2.Href, StringComparison.OrdinalIgnoreCase) == 0 &&
                         string.Compare(l1.Rel, l2.Rel, StringComparison.OrdinalIgnoreCase) == 0;
            return result;
        }

        /// <summary>
        /// Gets hashcode of the <see cref="Link" /> instance.
        /// </summary>
        /// <param name="lnk">The <see cref="Link" /> instance.</param>
        /// <returns>Returns the hashcode generated.</returns>
        public int GetHashCode(Link lnk)
        {
            var str = (string.IsNullOrEmpty(lnk.Rel) ? "norel" : lnk.Rel) + "~" + (string.IsNullOrEmpty(lnk.Href) ? "nohref" : lnk.Href);
            var h = str.GetHashCode();
            return h;
        }
    }
}