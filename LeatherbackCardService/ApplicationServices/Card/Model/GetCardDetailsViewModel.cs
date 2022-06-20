using System;
using System.Collections.Generic;
using Domain.Entities.Enums;

namespace ApplicationServices.Card.Model
{
    public class GetCardDetailsViewModel
    {
        public Guid Id { get; set; }
        public string CardHolderName { get; set; }
        public CardStatus CardStatus { get; set; }
        public CardType CardType { get; set; }
        public string CurrencyCode { get; set; }
        public string CardDesign { get; set; }
        public string CardProgramme { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid AccountId { get; set; }
        public Guid? RequestId { get; set; }
        public string MaskedPan { get; set; }
        public string ExpiryDate { get; set; }
        public Domain.Entities.Customers.Customer Customer { get; set; }
        public List<CardLimitViewModel> CardLimits { get; set; }
    }
}