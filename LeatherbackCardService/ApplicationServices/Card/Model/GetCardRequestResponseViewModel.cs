using System;
using System.Collections.Generic;
using Domain.Entities.Enums;

namespace ApplicationServices.Card.Model
{
    public class GetCardRequestResponseViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryIso { get; set; }
        public string PostalCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CountryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CardLimitViewModel> CardLimits { get; set; }
        
        public CardDesignType Design { get; set; }
        
        public string CardDeliveryName { get; set; }
        public CardType CardType { get; set; }
        public CustomerType CustomerType { get; set; }
        public CardRequestStatus Status { get; set; }
    }
}