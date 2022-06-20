using System;
using Domain.Entities.Enums;

namespace ApplicationServices.Card.Model
{
    public class GetCardViewModel
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string Cvv { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string CardIdentifier { get; set; }
        public string CardQrCodeContent { get; set; }
        public CardStatus CardStatus { get; set; }
        public string CardStatusReason { get; set; }
        
        public string ExpiryDate { get; set; }
        
        public string MaskedPan { get; set; }
    }
}