using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace GraphQLExample.Infrastructure.Mongo
{
    public class MongoDbInitializer : IDatabaseInitializer
    {
        private readonly IMongoDatabase _database;
        private readonly IDatabaseSeeder _seeder;
        private readonly bool _seed;
        private readonly bool _recreateDatabase;
        private bool _initialized;

        public MongoDbInitializer(IMongoDatabase database,
            IDatabaseSeeder seeder,
            MongoSettings options)
        {
            _database = database;
            _seeder = seeder;
            _seed = options.Seed;
            _recreateDatabase = options.RecreateDatabase;
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;

            RegisterConventions();
            _initialized = true;

            if (_recreateDatabase)
            {
                var collections = await _database.ListCollectionNamesAsync();
                await collections.ForEachAsync(async collection => await _database.DropCollectionAsync(collection));
            }

            if (!_seed) return;
            if (await _seeder.HasCollectionsAsync()) return;

            await _seeder.SeedAsync();
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("Conventions", new MongoDbConventions(), x => true);
        }

        private class MongoDbConventions : IConventionPack
        {
            public IEnumerable<IConvention> Conventions
                => new List<IConvention>
                {
                    new IgnoreExtraElementsConvention(true),
                    new EnumRepresentationConvention(BsonType.String),
                    new CamelCaseElementNameConvention()
                };
        }
    }
}