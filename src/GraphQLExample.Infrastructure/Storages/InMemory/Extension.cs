using Autofac;
using GraphQLExample.Common.Types;

namespace GraphQLExample.Infrastructure.InMemory
{
    public static class Extension
    {
        public static void AddInMemoryRepository<TEntity>(this ContainerBuilder builder)
            where TEntity : IIdentifiable
            => builder.RegisterType<InMemoryRepository<TEntity>>()
                .AsImplementedInterfaces()
                .SingleInstance();
    }
}