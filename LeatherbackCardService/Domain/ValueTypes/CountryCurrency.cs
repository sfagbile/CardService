using Domain.Exceptions;

namespace Domain.ValueTypes
{
    public record CountryCurrency
    {
        public string Country { get; set; } 
        public string Currency { get; set; }

        public CountryCurrency(string currency, string country)
        {
            if (string.IsNullOrEmpty(currency))
                throw new InvalidMoneyMobileDetailException("Currency can not be null");
            Currency = currency;
            if (string.IsNullOrEmpty(country))
                throw new InvalidMoneyMobileDetailException("Currency can not be null");
            Country = country;
        }
    }
}