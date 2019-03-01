using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GraphQLExample.Common.Types;
using GraphQLExample.Infrastructure;

namespace GraphQLExample.Infrastructure
{
    public class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : IIdentifiable
    {
        private readonly List<TEntity> _collection = new List<TEntity>();

        public async Task CreateAsync(TEntity entity)
            => await Task.Run(() => _collection.Add(entity));

        public async Task DeleteAsync(Guid id)
            => await Task.Run(() => _collection.RemoveAll(entity => entity.Id.Equals(id)));

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
            => await Task.FromResult(_collection.AsQueryable().Any(predicate));

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await Task.FromResult(_collection.AsQueryable().Where(predicate));

        public async Task<TEntity> GetAsync(Guid id)
            => await Task.FromResult(_collection.FirstOrDefault(entity => entity.Id.Equals(id)));

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
            => await Task.FromResult(_collection.AsQueryable().FirstOrDefault(predicate));

        public IQueryable<TEntity> Query()
            => new List<TEntity>(_collection).AsQueryable();

        public async Task UpdateAsync(TEntity entity)
        {
            await DeleteAsync(entity.Id);
            await CreateAsync(entity);
        }
    }
}