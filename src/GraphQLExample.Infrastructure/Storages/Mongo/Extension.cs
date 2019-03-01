using Autofac;
using GraphQLExample.Common.Types;
using GraphQLExample.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace GraphQLExample.Infrastructure.Mongo
{
    public static class Extension
    {
        public static void AddMongoDb(this ContainerBuilder builder, string sectionName)
        {
            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var options = configuration.GetOptions<MongoSettings>(sectionName);
                return options;
            }).SingleInstance();

            builder.Register(context =>
            {
                var options = context.Resolve<MongoSettings>();
                var settings = new MongoClientSettings
                {
                    Server = new MongoServerAddress(options.Host, options.Port),
                    GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard
                };

                return new MongoClient(settings);
            }).SingleInstance();

            builder.Register(context =>
            {
                var options = context.Resolve<MongoSettings>();
                var client = context.Resolve<MongoClient>();
                return client.GetDatabase(options.DatabaseName);
            }).InstancePerLifetimeScope();
        }

        public static void AddMongoDbRepository<TEntity>(this ContainerBuilder builder, string collectionName)
            where TEntity : IIdentifiable
            => builder.Register(ctx => new MongoRepository<TEntity>(ctx.Resolve<IMongoDatabase>(), collectionName))
                .AsImplementedInterfaces()
                .SingleInstance();
    }
}