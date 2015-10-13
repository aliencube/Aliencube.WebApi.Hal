using System;
using System.Collections.Generic;

using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Converters
{
    /// <summary>
    /// This represents the equality comparer entity for <see cref="LinkedResource" />.
    /// </summary>
    internal sealed class LinkedResourceEqualityComparer : IEqualityComparer<LinkedResource>
    {
        /// <summary>
        /// Check whether both given resources are equal to each other or not.
        /// </summary>
        /// <param name="r1">First <see cref="LinkedResource" /> instance.</param>
        /// <param name="r2">Second <see cref="LinkedResource" /> instance. </param>
        /// <returns>
        /// Returns <c>True</c>, if both resources are the same as each other; otherwise returns <c>False</c>.
        /// </returns>
        public bool Equals(LinkedResource r1, LinkedResource r2)
        {
            var result = string.Equals(r1.Rel, r2.Rel, StringComparison.InvariantCultureIgnoreCase) &&
                         string.Equals(r1.Href, r2.Href, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets hash code of the <see cref="LinkedResource" /> instance.
        /// </summary>
        /// <param name="resource">The <see cref="LinkedResource" /> instance.</param>
        /// <returns>Returns the hash code generated.</returns>
        public int GetHashCode(LinkedResource resource)
        {
            var norel = string.IsNullOrWhiteSpace(resource.Rel) ? "norel" : resource.Rel;
            var nohref = string.IsNullOrWhiteSpace(resource.Href) ? "nohref" : resource.Href;
            var joined = string.Join("~", norel, nohref);
            var hash = joined.GetHashCode();
            return hash;
        }
    }
}