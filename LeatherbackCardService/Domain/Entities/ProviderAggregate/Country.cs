using System;
using Shared;
using Shared.BaseResponse;
using Shared.Exceptions;

namespace Domain.Entities.ProviderAggregate
{
    public class Country : Entity<Guid>
    {
        public string Name { get; set; }
        public string Iso { get; set; }
        public string Iso3 { get; set; }
        public string Region { get; set; }

        protected override void When(object @event)
        {
            throw new System.NotImplementedException();
        }

        private Country()
        {
        }

        private Country(string name, string iso) 
        {
            Name = name;
            Iso = iso;
        }

        public static Result<Country> Create(string name, string iso)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidEntityException("Name of currency can not be null or empty");

            if (string.IsNullOrEmpty(iso))
                throw new InvalidEntityException("Iso of currency can not be null or empty");

            return Result.Ok(new Country(name, iso));
        }
    }
}