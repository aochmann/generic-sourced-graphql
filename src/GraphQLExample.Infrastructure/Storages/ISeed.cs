using System.Threading.Tasks;

namespace GraphQLExample.Infrastructure
{
    public interface ISeed
    {
        Task SeedAsync();
    }
}