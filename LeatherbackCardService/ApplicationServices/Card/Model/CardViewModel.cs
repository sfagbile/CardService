using System;
using System.Collections.Generic;
using Domain.Entities.Enums;

namespace ApplicationServices.Card.Model
{
    public class CardViewModel
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
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public  CardStatus Status { get; set; }
        public string MaskedNumber { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CardLimitViewModel> CardLimits { get; set; }
        public string ExpiryDate { get; set; }
    }
}