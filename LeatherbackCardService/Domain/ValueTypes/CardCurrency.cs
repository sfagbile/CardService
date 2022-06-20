using Domain.Exceptions;

namespace Domain.ValueTypes
{
    public record CardCurrency
    {
        public string Country { get; init; }
        public string Currency { get; init; }

        public CardCurrency(string currency, string country)
        {
            if (string.IsNullOrEmpty(currency))
                throw new InvalidCardException("Currency can not be null");
            Currency = currency;
            if (string.IsNullOrEmpty(country))
                throw new InvalidCardException("Country can not be null");
            Country = country;
        }
    }
}