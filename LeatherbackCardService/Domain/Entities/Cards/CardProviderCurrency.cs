using System;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using Shared;
using Shared.BaseResponse;
using Shared.Exceptions;

namespace Domain.Entities.Cards
{
    public class CardProviderCurrency : Entity<Guid>
    {
        private CardProviderCurrency()
        {
        }

        private CardProviderCurrency(Guid id, Guid cardProviderId, Guid currencyId,
            Guid countryId, CustomerType customerType, CardType cardType, string cardDesign, string cardProgramme)
        {
            Id = id;
            CardProviderId = cardProviderId;
            CurrencyId = currencyId;
            CountryId = countryId;
            CustomerType = customerType;
            CardType = cardType;
            CardDesign = cardDesign;
            CardProgramme = cardProgramme;
        }

        public Guid CardProviderId { get; set; }
        public CardProvider CardProvider { get; set; }

        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public Guid CountryId { get; set; }
        public Country Country { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsPrimary { get; set; }

        public string CardDesign { get; set; }

        public string CardProgramme { get; set; }

        public CustomerType CustomerType { get; set; }
        public CardType CardType { get; set; }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }

        public static Result<CardProviderCurrency> Create(Guid cardProviderId, Guid currencyId, Guid countryId,
            CustomerType customerType, CardType cardType,  string cardDesign, string cardProgramme)
        {
            return Result.Ok(
                new CardProviderCurrency(Guid.NewGuid(), cardProviderId, currencyId, countryId, customerType, cardType, cardDesign, cardProgramme));
        }
    }
}