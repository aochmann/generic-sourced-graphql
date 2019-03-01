using System;
using GraphQLExample.Common.Types;

namespace GraphQLExample.Models.Users
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public User(Guid id) : base(id)
        {
        }
    }
}