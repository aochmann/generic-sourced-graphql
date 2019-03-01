using System.Threading.Tasks;
using GraphQLExample.Infrastructure;

namespace GraphQLExample.Api.Seeds
{
    public class DevSeed : ISeed
    {
        public async Task SeedAsync()
        {
            await Task.Delay(100);
        }
    }
}