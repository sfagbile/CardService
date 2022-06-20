using Domain.Exceptions;

namespace Domain.ValueTypes
{
    public record CardExpiration
    {
        public string Month { get; init; }
        public string Year { get; init; }

        public CardExpiration(string month, string year)
        {
            Month = month;
            Year = year;
        }
    }
}