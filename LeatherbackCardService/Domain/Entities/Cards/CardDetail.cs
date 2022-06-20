using System;
using Domain.Entities.Customers;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using Shared;
using Shared.BaseResponse;

namespace Domain.Entities.Cards
{
    public class CardDetail : Entity<Guid>
    {
        private CardDetail( Guid id, Guid providerEndUserId, Guid currencyId, CardType cardType, CardRequestStatus status,
            Guid cardRequestId, string cardDesign, string cardProgramme, Guid accountId)
        {
            Id = id;
            CurrencyId = currencyId;
            CardType = cardType;
            Status = status;
            CardRequestId = cardRequestId;
            CardDesign = cardDesign;
            CardProgramme = cardProgramme;
            ProviderEndUserId = providerEndUserId;
            AccountId = accountId;
        }

        public Guid ProviderEndUserId { get; set; }
        public string ProviderLedgerId { get; set; }
        public Guid AccountId { get; set; }
        public ProviderEndUser ProviderEndUser { get; set; }
        public CardType CardType { get; set; }
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public Guid CardRequestId { get; set; }
        public CardRequest CardRequest { get; set; }
        public string CardDesign { get; set; }
        public string CardProgramme { get; set; }
        public CardRequestStatus Status { get; set; }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }

        public static Result<CardDetail> Create( Guid providerEndUserId,Guid currencyId, CardType cardType, CardRequestStatus status,
            Guid cardRequestId, string cardDesign, string cardProgramme, Guid accountId)
        {
            if (currencyId == default)
                return Result.Fail<CardDetail>($"{nameof(currencyId)} is invalid");

            if (cardType == default) return Result.Fail<CardDetail>($"{nameof(cardType)} is invalid");

            if (status == default) return Result.Fail<CardDetail>($"{nameof(status)} is invalid");
            
            if (providerEndUserId == default) return Result.Fail<CardDetail>($"{nameof(providerEndUserId)} is invalid");
            if (accountId == default) return Result.Fail<CardDetail>($"{nameof(accountId)} is invalid");
            
            return Result.Ok(new CardDetail( Guid.NewGuid(), providerEndUserId, currencyId, cardType, status, cardRequestId, cardDesign,
                cardProgramme, accountId));
        }
    }
}