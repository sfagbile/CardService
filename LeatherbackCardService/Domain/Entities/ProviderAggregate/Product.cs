using System;
using Shared;
using Shared.BaseResponse;
using Shared.Exceptions;

namespace Domain.Entities.ProviderAggregate
{
    public class Product : Entity<Guid>
    {
        private Product()
        {
        }

        private Product(Guid id, string name, string code)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
        public string Description { get; set; }


        public static Result<Product> Create(Guid id, string name, string code)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidEntityException($"{nameof(name)} can not be null or empty");

            if (string.IsNullOrEmpty(code))
                throw new InvalidEntityException($"{nameof(code)} can not be null or empty");

            return Result.Ok(new Product(id, name, code));
        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}