using Microsoft.Extensions.Configuration;

namespace GraphQLExample.Infrastructure.Options
{
    public static class Extension
    {
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName) where TModel : new()
        {
            var model = new TModel();
            
            configuration
                .GetSection(sectionName)
                .Bind(model);

            return model;
        }
    }
}