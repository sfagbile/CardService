using System;
using Shared;
using Shared.BaseResponse;
using Shared.Exceptions;

namespace Domain.Entities.ProviderAggregate
{
    public class CardProvider : Entity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Branch { get; set; }
        public string Postcode { get; set; }
        public string Address { get; set; }
        public Guid CountryId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public bool HasWebhook { get; set; }
        public Country Country { get; }

        protected override void When(object @event)
        {
            throw new System.NotImplementedException();
        }

        private CardProvider(Guid id, string name, string description, string email) : base(id)
        {
            Name = name;
            Description = description;
            Email = email;
        }

        public static Result<CardProvider> Create(Guid id, string name, string description, string email)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidEntityException("Name can not be null or empty");

            return Result.Ok(new CardProvider(id, name, description, email));
        }
    }
}