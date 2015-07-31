using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using Aliencube.WebApi.Hal.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aliencube.WebApi.Hal.Formatters
{
    public class HalJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public HalJsonMediaTypeFormatter()
        {
            this.SetSupportedMediaTypes();
        }

        public override bool CanReadType(Type type)
        {
            var isSupportedType = IsSupportedType(type);
            return isSupportedType;
        }

        public override bool CanWriteType(Type type)
        {
            var isSupportedType = IsSupportedType(type);
            return isSupportedType;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, Encoding effectiveEncoding)
        {
            base.WriteToStream(type, value, writeStream, effectiveEncoding);

            var formatting = this.Indent ? Formatting.Indented : Formatting.None;
            var so = this.SerializerSettings == null
                ? JsonConvert.SerializeObject(value, formatting)
                : JsonConvert.SerializeObject(value, formatting, this.SerializerSettings);

            var jo = JObject.Parse(so);

            var resource = value as LinkedResource;
            if (resource == null)
            {
                return;
            }

            var links = resource.Links.Select(p => new KeyValuePair<string, object>(p.Rel, new { href = p.Href }));
            var sls = this.SerializerSettings == null
                ? JsonConvert.SerializeObject(links, formatting)
                : JsonConvert.SerializeObject(links, formatting, this.SerializerSettings);

            var jlo = JObject.Parse(sls);
            jo["_links"] = jlo;

            using (var sw = new StreamWriter(writeStream))
            using (var writer = new JsonTextWriter(sw))
            {
                jo.WriteTo(writer);
            }
        }

        //public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
        //    TransportContext transportContext)
        //{
        //    return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        //}

        //public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
        //    TransportContext transportContext, CancellationToken cancellationToken)
        //{
        //    return base.WriteToStreamAsync(type, value, writeStream, content, transportContext, cancellationToken);
        //}

        private void SetSupportedMediaTypes()
        {
            this.SupportedMediaTypes.Clear();
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
        }

        private static bool IsSupportedType(Type type)
        {
            var isLinkedResourceType = type.IsSubclassOf(typeof(LinkedResource));
            return isLinkedResourceType;
        }
    }
}