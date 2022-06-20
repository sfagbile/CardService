using Shared.Exceptions;

namespace Domain.ValueTypes
{
    public record ProviderCountry
    {
        public string Name { get; init; }
        public string Iso { get; init; }

        public ProviderCountry(string name, string iso)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidEntityException("Name must not be null or empty");
            if (string.IsNullOrEmpty(iso))
                throw new InvalidEntityException("Iso must not be null or empty");
        }
    }
}