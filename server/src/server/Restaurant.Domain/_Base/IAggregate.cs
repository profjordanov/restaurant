using System;

namespace Restaurant.Domain._Base
{
    public interface IAggregate
    {
        Guid Id { get; set; }
    }
}
