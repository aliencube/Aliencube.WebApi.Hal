using System;
using System.Net.Http.Formatting;

namespace Aliencube.WebApi.Hal.Formatters
{
    public class HalJsonMediaTypeFormatter : BufferedMediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            throw new NotImplementedException();
        }

        public override bool CanWriteType(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
