using System;
using Domain.Entities.Cards;
using Shared;
using Shared.BaseResponse;

namespace Domain.Entities.Transactions
{
    public class CardLimit : Entity<Guid>
    {
        private CardLimit(Guid cardLimitTypeId, Guid cardId, decimal minAmount, decimal maxAmount)
        {
            CardLimitTypeId = cardLimitTypeId;
            CardId = cardId;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }

        public static Result<CardLimit> CreateInstance(Guid cardLimitTypeId, Guid cardId,
            decimal maxAmount, decimal minAmount = Decimal.Zero)
        {
            if (cardLimitTypeId == default)
                return Result.Fail<CardLimit>($"{nameof(cardLimitTypeId)} is invalid");
            
            if (cardId == default)
                return Result.Fail<CardLimit>($"{nameof(cardId)} is invalid");

            if (maxAmount == default)
                return Result.Fail<CardLimit>($"{nameof(maxAmount)} is invalid");
            
            return Result.Ok(new CardLimit(cardLimitTypeId, cardId, minAmount, maxAmount));
        }

        public Guid CardLimitTypeId { get; set; }
        public CardLimitType CardLimitTypes { get; set; }
        public Guid CardId { get; set; }
        public Card Card { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}