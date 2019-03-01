using System;

namespace GraphQLExample.Common.Types
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}