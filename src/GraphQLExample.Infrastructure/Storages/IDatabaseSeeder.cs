using System.Threading.Tasks;

namespace GraphQLExample.Infrastructure
{
    public interface IDatabaseSeeder
    {
         Task<bool> HasCollectionsAsync();
         Task SeedAsync();
    }
}