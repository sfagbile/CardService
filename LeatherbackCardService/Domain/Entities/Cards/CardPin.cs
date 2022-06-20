using System;
using Shared;
using Shared.Utility;

namespace Domain.Entities.Cards
{
    public class CardPin : Entity<Guid>
    {
        private CardPin(Guid id, Guid cardId, string pin)
        {
            Id = id;
            CardId = cardId;
            Pin = pin;
        }

        public static CardPin CreateInstance(Guid cardId, string pin)
        {
            var Id = Guid.NewGuid();
            var encryptedPin = EncryptionUtil.GenerateSha512Hash(cardId + pin + Id);
            return new CardPin(Id, cardId, encryptedPin);
        }

        public Guid CardId { get; set; }
        public string Pin { get; set; }
        
        
        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}