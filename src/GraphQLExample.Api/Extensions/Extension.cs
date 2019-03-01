using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GraphQLExample.Api.Extensions
{
    public static class Extension
    {
        public static IMvcCoreBuilder AddDefaultJsonOptions(this IMvcCoreBuilder builder)
            => builder.AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opts.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    opts.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
                    opts.SerializerSettings.Formatting = Formatting.Indented;
                });
    }
}