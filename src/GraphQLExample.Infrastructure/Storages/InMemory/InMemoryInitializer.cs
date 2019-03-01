using System.Threading.Tasks;

namespace GraphQLExample.Infrastructure.InMemory
{
    public class InMemoryInitializer : IDatabaseInitializer
    {
        private bool _initialized;
        private readonly IDatabaseSeeder _seeder;

        public InMemoryInitializer(IDatabaseSeeder seeder)
        {
            _seeder = seeder;
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;
            _initialized = true;

            await _seeder.SeedAsync();
        }
    }
}