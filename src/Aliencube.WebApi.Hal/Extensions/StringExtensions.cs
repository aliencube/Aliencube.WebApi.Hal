namespace Aliencube.WebApi.Hal.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="string" /> value.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string value to camelCase.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns the string value converted to camelCase.</returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            if (value.Length == 1)
            {
                return value.ToLowerInvariant();
            }

            var first = value.Substring(0, 1);
            var rest = value.Substring(1);

            return first.ToLowerInvariant() + rest;
        }
    }
}
