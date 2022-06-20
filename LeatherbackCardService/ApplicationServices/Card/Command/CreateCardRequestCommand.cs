using System;
using System.Collections.Generic;
using ApplicationServices.Card.Model;
using Domain.Entities.Enums;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Command
{
    public class CreateCardRequestCommand : IRequest<Result<CardRequestResponseViewModel>>
    {
        public Guid CustomerId { get; set; }

        public Guid AccountId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string CountryIso { get; set; }

        public string PostalCode { get; set; }

        public string CurrencyCode { get; set; }

        public CardType CardType { get; set; }

        public CustomerType CustomerType { get; set; }
        public string Product { get; set; }

        public CardDesignType Design { get; set; }

        public string CardDeliveryName { get; set; }

        public List<CardLimitViewModel> CardLimit { get; set; }
    }
}