using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GraphQLExample.Infrastructure.Mongo
{
    public class MongoDbSeeder : IDatabaseSeeder
    {
        private readonly IMongoDatabase _database;
        private readonly ISeed _seed;

        public MongoDbSeeder(IMongoDatabase database, ISeed seed)
        {
            _database = database;
            _seed = seed;
        }

        public async Task<bool> HasCollectionsAsync()
        {
            var cursor = await _database.ListCollectionsAsync();
            var collections = await cursor.ToListAsync();

            return collections.Any();
        }

        public async Task SeedAsync()
            => await _seed.SeedAsync();
    }
}