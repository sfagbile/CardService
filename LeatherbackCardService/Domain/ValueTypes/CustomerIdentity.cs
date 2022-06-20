using System;
using Shared.Exceptions;

namespace Domain.ValueTypes
{
    public record CustomerIdentity
    {
        public Guid CustomerId { get; init; }
        public CustomerIdentity(Guid customerId)
        {
            if (customerId == default)
                throw new InvalidEntityException("Not a valid Customer");
            CustomerId = customerId;
        }
    }
}