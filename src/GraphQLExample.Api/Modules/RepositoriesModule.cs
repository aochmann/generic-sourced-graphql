using Autofac;
using GraphQLExample.Infrastructure.InMemory;
using GraphQLExample.Models.Orders;
using GraphQLExample.Models.Products;
using GraphQLExample.Models.Users;

namespace GraphQLExample.Api.Modules
{
    public class RepositoriesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddInMemoryRepository<User>();
            builder.AddInMemoryRepository<Order>();
            builder.AddInMemoryRepository<Product>();
        }
    }
}