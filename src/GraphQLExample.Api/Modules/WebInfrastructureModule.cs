using System;
using Autofac;
using GraphQLExample.Infrastructure;
using GraphQLExample.Infrastructure.InMemory;
using GraphQLExample.Infrastructure.Mongo;
using GraphQLExample.Infrastructure.Storages;
using Microsoft.Extensions.Hosting;

namespace GraphQLExample.Api.Modules
{
    public class WebInfrastructureModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StorageSelector>()
                .AsSelf()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(StorageType), StorageType.InMemory));

            builder.Register(ctx => {
                var selector = ctx.Resolve<StorageSelector>();

                IDatabaseInitializer initializer = null;

                switch (selector.StorageType)
                {
                    case StorageType.InMemory:
                        initializer = ctx.Resolve<InMemoryInitializer>();
                    break;
                    case StorageType.Mongo:
                        initializer = ctx.Resolve<MongoDbInitializer>(); 
                    break;
                    default:
                        throw new NotImplementedException($"Initializer: Not implamented {selector.StorageType}");
                }

                return initializer;
            })
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

            builder.Register(ctx => {
                var selector = ctx.Resolve<StorageSelector>();

                IDatabaseSeeder seeder = null;

                switch (selector.StorageType)
                {
                    case StorageType.InMemory:
                        seeder = ctx.Resolve<InMemorySeeder>();
                    break;
                    case StorageType.Mongo:
                        seeder = ctx.Resolve<MongoDbSeeder>(); 
                    break;
                    default:
                        throw new NotImplementedException($"Seeder: Not implamented {selector.StorageType}");
                }

                return seeder;
            })
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

            builder.Register(ctx =>
            {
                var env = ctx.Resolve<IHostingEnvironment>();

                return env.IsDevelopment()
                    ? ctx.Resolve<Seeds.DevSeed>()
                    : ctx.Resolve<Seeds.ProdSeed>() as ISeed;
            }).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}