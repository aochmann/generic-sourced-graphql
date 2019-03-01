using System;
using GraphQLExample.Common.Types;
using GraphQLExample.Models.Products;

namespace GraphQLExample.Models.Orders
{
    public class Order : BaseEntity
    {
        public Product Product { get; }

        public Order(Guid id, Product product) : base(id)
            =>  Product = product;
    }
}