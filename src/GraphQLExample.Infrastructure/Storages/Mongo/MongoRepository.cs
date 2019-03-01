using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GraphQLExample.Common.Types;
using MongoDB.Driver;

namespace GraphQLExample.Infrastructure.Mongo
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : IIdentifiable
    {
        public IMongoCollection<TEntity> Collection { get; }

        public MongoRepository(IMongoDatabase database, string collectionName)
            => Collection = database.GetCollection<TEntity>(collectionName);

        public async Task<TEntity> GetAsync(Guid id)
            => await GetAsync(e => e.Id == id);

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
            => await Collection.Find(predicate).SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await Collection.Find(predicate).ToListAsync();

        public async Task CreateAsync(TEntity entity)
            => await Collection.InsertOneAsync(entity);

        public async Task UpdateAsync(TEntity entity)
            => await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);

        public async Task DeleteAsync(Guid id)
            => await Collection.DeleteOneAsync(e => e.Id == id);

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
            => await Collection.Find(predicate).AnyAsync();

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
            => await Collection.Find(predicate).FirstOrDefaultAsync();

        public async Task<IClientSessionHandle> BeginSession()
            => await Collection.Database.Client.StartSessionAsync();

        public IQueryable<TEntity> Query()
            => Collection.AsQueryable();

        public async Task CreateAsync(TEntity entity, IClientSessionHandle session)
            => await Collection.InsertOneAsync(session, entity);

        public async Task UpdateAsync(TEntity entity, IClientSessionHandle session)
            => await Collection.ReplaceOneAsync(session, e => e.Id == entity.Id, entity);

        public async Task DeleteAsync(Guid id, IClientSessionHandle session)
            => await Collection.DeleteOneAsync(session, e => e.Id == id);
    }
}