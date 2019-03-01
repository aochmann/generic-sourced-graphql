using System;
using GraphQLExample.Common.Types;

namespace GraphQLExample.Models.Products
{
    public class Product : BaseEntity
    {
        public string ProductName { get; }
        
        public Product(Guid id, string productName) : base(id)
        {
            ProductName = productName;
        }

    }
}