namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This determines embedded type to be used during the serialisation.
    /// </summary>
    public enum EmbeddedType
    {
        /// <summary>
        /// Indicates that embedded type is not used.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that embedded type is used.
        /// </summary>
        Embedded = 1,
    }
}