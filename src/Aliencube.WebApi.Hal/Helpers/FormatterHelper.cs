using System;

using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Helpers
{
    /// <summary>
    /// This represents the helper entity for formatters.
    /// </summary>
    public static class FormatterHelper
    {
        /// <summary>
        /// Checks whether the given type is either <see cref="LinkedResource" /> or <see cref="LinkedResourceCollection{T}" />.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the given type is either <see cref="LinkedResource" /> or <see cref="LinkedResourceCollection{T}" />; otherwise returns <c>False</c>.</returns>
        public static bool IsSupportedType(Type type)
        {
            var isLinkedResourceType = IsLinkedResourceType(type);
            var isLinkedResourceCollectionType = IsLinkedResourceCollectionType(type);

            return isLinkedResourceType || isLinkedResourceCollectionType;
        }

        /// <summary>
        /// Checks whether the given type is <see cref="LinkedResource" />.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the given type is either <see cref="LinkedResource" />; otherwise returns <c>False</c>.</returns>
        public static bool IsLinkedResourceType(Type type)
        {
            return type.IsSubclassOf(typeof(LinkedResource));
        }

        /// <summary>
        /// Checks whether the given type is <see cref="LinkedResourceCollection{T}" />.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the given type is <see cref="LinkedResourceCollection{T}" />; otherwise returns <c>False</c>.</returns>
        public static bool IsLinkedResourceCollectionType(Type type)
        {
            var typeToCheck = type;
            while (typeToCheck != null && typeToCheck != typeof(object))
            {
                var currentType = typeToCheck.IsGenericType ? typeToCheck.GetGenericTypeDefinition() : typeToCheck;
                if (currentType == typeof(LinkedResourceCollection<>))
                {
                    return true;
                }

                typeToCheck = typeToCheck.BaseType;
            }

            return false;
        }
    }
}