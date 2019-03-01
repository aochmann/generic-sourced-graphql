using System;
using System.Threading.Tasks;
using GraphQLExample.Common.Types;
using MongoDB.Driver;

namespace GraphQLExample.Infrastructure.Mongo
{
    public interface IMongoRepository<TEntity> : IRepository<TEntity> where TEntity : IIdentifiable
    {
        Task CreateAsync(TEntity entity, IClientSessionHandle session);

        Task UpdateAsync(TEntity entity, IClientSessionHandle session);

        Task DeleteAsync(Guid id, IClientSessionHandle session);
    }
}