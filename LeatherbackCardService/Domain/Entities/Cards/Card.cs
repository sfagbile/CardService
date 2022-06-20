using System;
using Domain.Entities.Enums;
using Domain.ValueTypes;
using Shared;
using Shared.BaseResponse;

namespace Domain.Entities.Cards
{
    public class Card : Entity<Guid>
    {
        public string CardNumber { get; init; }
        public string CardHolderName { get; set; }
        public string Cvv { get; init; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public bool IsPinIssued { get; set; }
        public string CardToken { get; set; }
        public string ReissuedCardId { get; set; }
        public string ReissuedCardToken { get; set; }
        public string CardIdentifier { get; set; }
        public string CardQrCodeContent { get; set; }
        public CardStatus CardStatus { get; set; }
        public string CardStatusReason { get; set; }
        public CardCarrierType CardCarrierType { get; set; }
        public string ProviderResponse { get; set; }
        public Guid CardDetailId { get; init; }
        public string LeatherBackCardDesign { get; set; }
        public string MaskedPan { get; set; }
        public string ExpiryDate { get; set; }

        public CardDetail CardDetails { get; set; }

        private Card(Guid id, string cardNumber, string cvv,
             string cardHolderName, CardStatus cardStatus, string cardStatusReason,
            CardCarrierType cardCarrierType, Guid cardDetailId, string expireMonth, string expireYear, string leatherBackCardDesign) : base(id)
        {
            Id = id;
            CardNumber = cardNumber;
            Cvv = cvv;
            CardHolderName = cardHolderName;
            CardStatus = cardStatus;
            CardStatusReason = cardStatusReason;
            CardCarrierType = cardCarrierType;
            CardDetailId = cardDetailId;
            ExpireMonth = expireMonth;
            ExpireYear = expireYear;
            LeatherBackCardDesign = leatherBackCardDesign;
        }

        public static Card IssuePin(Card card)
        {
            card.IsPinIssued = true;
            return card;
        }

        public static Result<Card> CreateCard(string cardNumber, string cvv,
            CardExpiration cardExpiration, string cardHolderName, CardStatus cardStatus, string cardStatusReason,
            CardCarrierType cardCarrierType,  Guid cardDetailId, string leatherBackCardDesign)
        {

            if (cardDetailId == default)
                return Result.Fail<Card>($"{nameof(cardDetailId)} can not be null");
            
            if (string.IsNullOrEmpty(cardHolderName))
                return Result.Fail<Card>($"{nameof(cardHolderName)} can not be null");
            
            if (string.IsNullOrEmpty(leatherBackCardDesign))
                return Result.Fail<Card>($"{nameof(leatherBackCardDesign)} can not be null");

            return Result.Ok(new Card(Guid.NewGuid(), cardNumber, cvv, cardHolderName, cardStatus, cardStatusReason,
                cardCarrierType, cardDetailId, cardExpiration.Month, cardExpiration.Year, leatherBackCardDesign));
        }
        
        public Card SetCardIdentifier(string cardIdentifier)
        {
            this.CardIdentifier = cardIdentifier;
            return this;
        }

        public Card SetCardQrCodeContent(string cardQrCodeContent)
        {
            this.CardQrCodeContent = cardQrCodeContent;
            return this;
        }


        protected override void When(object @event)
        {
            switch (@event)
            {
                default:
                    throw new NotImplementedException();
                // break;
            }
        }
    }
}