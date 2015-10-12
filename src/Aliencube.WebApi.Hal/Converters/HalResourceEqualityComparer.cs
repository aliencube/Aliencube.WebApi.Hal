using System;
using System.Collections.Generic;

using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Converters
{
    /// <summary>
    /// This represents the equality comparer entity for <see cref="HalResource" />.
    /// </summary>
    internal sealed class HalResourceEqualityComparer : IEqualityComparer<HalResource>
    {
        /// <summary>
        /// Check whether both given resources are equal to each other or not.
        /// </summary>
        /// <param name="r1">First <see cref="HalResource" /> instance.</param>
        /// <param name="r2">Second <see cref="HalResource" /> instance. </param>
        /// <returns>
        /// Returns <c>True</c>, if both resources are the same as each other; otherwise returns <c>False</c>.
        /// </returns>
        public bool Equals(HalResource r1, HalResource r2)
        {
            var result = r1.Rel.Equals(r2.Rel, StringComparer.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets hash code of the <see cref="HalResource" /> instance.
        /// </summary>
        /// <param name="resource">The <see cref="HalResource" /> instance.</param>
        /// <returns>Returns the hash code generated.</returns>
        public int GetHashCode(HalResource resource)
        {
            var hash = resource.Rel.GetHashCode();
            return hash;
        }
    }
}