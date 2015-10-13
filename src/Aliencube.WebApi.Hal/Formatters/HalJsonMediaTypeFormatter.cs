using System.Net.Http.Formatting;
using System.Net.Http.Headers;

using Aliencube.WebApi.Hal.Converters;

namespace Aliencube.WebApi.Hal.Formatters
{
    /// <summary>
    /// This represents the formatter entity to support JSON response format with HAL.
    /// </summary>
    public class HalJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private readonly LinkCollectionConverter _linkCollectionConverter;
        private readonly LinkedResourceCollectionConverter _halResourceCollectionConverter;

        /// <summary>
        /// Initialises a new instance of the <see cref="HalJsonMediaTypeFormatter" /> class.
        /// </summary>
        public HalJsonMediaTypeFormatter()
        {
            this._linkCollectionConverter = new LinkCollectionConverter();
            this._halResourceCollectionConverter = new LinkedResourceCollectionConverter();

            this.SetSupportedMediaTypes();
            this.Initialize();
        }

        private void Initialize()
        {
            this.SerializerSettings.Converters.Add(this._linkCollectionConverter);
            this.SerializerSettings.Converters.Add(this._halResourceCollectionConverter);
        }

        ///// <summary>
        ///// Determines whether the <see cref="HalJsonMediaTypeFormatter" /> can read objects of the specified type.
        ///// </summary>
        ///// <param name="type">The type of object that will be read.</param>
        ///// <returns>
        ///// Returns <c>True</c>, if objects of this type can be read, otherwise returns <c>False</c>.
        ///// </returns>
        //public override bool CanReadType(Type type)
        //{
        //    if (type == null)
        //    {
        //        return false;
        //    }

        //    var isSupportedType = FormatterHelper.IsSupportedType(type);
        //    return isSupportedType;
        //}

        ///// <summary>
        ///// Determines whether the <see cref="HalJsonMediaTypeFormatter" /> can write objects of the specified type.
        ///// </summary>
        ///// <param name="type">The type of object that will be written.</param>
        ///// <returns>
        ///// Returns <c>True</c>, if objects of this type can be written, otherwise returns <c>False</c>.
        ///// </returns>
        //public override bool CanWriteType(Type type)
        //{
        //    if (type == null)
        //    {
        //        return false;
        //    }

        //    var isSupportedType = FormatterHelper.IsSupportedType(type);
        //    return isSupportedType;
        //}

        ///// <summary>
        ///// Called during serialization to write an object of the specified type to the specified stream.
        ///// </summary>
        ///// <param name="type">The type of the object to write.</param>
        ///// <param name="value">The object to write.</param>
        ///// <param name="writeStream">The stream to write to.</param>
        ///// <param name="effectiveEncoding">The encoding to use when writing.</param>
        //public override void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding)
        //{
        //    if (type == null)
        //    {
        //        throw new ArgumentNullException(nameof(type));
        //    }

        //    if (value == null)
        //    {
        //        return;
        //    }

        //    if (writeStream == null)
        //    {
        //        throw new ArgumentNullException(nameof(writeStream));
        //    }

        //    if (effectiveEncoding == null)
        //    {
        //        throw new ArgumentNullException(nameof(effectiveEncoding));
        //    }

        //    var resource = value as LinkedResource;
        //    if (resource == null)
        //    {
        //        return;
        //    }

        //    var formatter = JsonResourceFormatter.Create(type, this.SerializerSettings);
        //    formatter.WriteToStream(type, value, writeStream, effectiveEncoding);
        //}

        private void SetSupportedMediaTypes()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
        }
    }
}