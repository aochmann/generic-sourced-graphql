using System.Threading.Tasks;

namespace GraphQLExample.Infrastructure.InMemory
{
    public class InMemorySeeder : IDatabaseSeeder
    {
        private readonly ISeed _seed;

        public InMemorySeeder(ISeed seed)
            => _seed = seed;

        public Task<bool> HasCollectionsAsync()
            => Task.FromResult(true);

        public async Task SeedAsync()
            => await _seed.SeedAsync();

    }
}