using System;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using Shared;
using Shared.BaseResponse;

namespace Domain.Entities.Cards
{
    public class CardRequest : Entity<Guid>
    {
        private CardRequest(Guid id, Guid customerId, string firstName, string lastName, string email,
            string address, DateTime dateOfBirth, string countryIso, CustomerType customerType,
            CardRequestStatus status, string phoneNumber, string currencyCode, Guid productId, CardType cardType,
            string postalCode, Guid accountId, string city, string middleName, CardDesignType design, string cardDeliveryName, string transactionLimit) : base(id)
        {
            Id = id;
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
            DateOfBirth = dateOfBirth;
            CountryIso = countryIso;
            CustomerType = customerType;
            Status = status;
            PhoneNumber = phoneNumber;
            CurrencyCode = currencyCode;
            ProductId = productId;
            CardType = cardType;
            PostalCode = postalCode;
            AccountId = accountId;
            City = city;
            MiddleName = middleName;
            Design = design;
            CardDeliveryName = cardDeliveryName;
            TransactionLimit = transactionLimit;
        }

        public Guid CustomerId { get; set; }

        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
       // public Sex Sex { get; set; }
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
        public CardRequestStatus Status { get; set; }
        public string CardRejectionReason { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        
        public CardDesignType Design { get; set; }
        
        public string CardDeliveryName { get; set; }
        public bool IsCreateCustomerInitiated { get; set; }
        public bool IsCreateCustomerSuccessful { get; set; }
        public string CreateCustomerResponse { get; set; }
        
        public bool IsCreateProviderEndUserInitiated { get; set; }
        public bool IsCreateProviderEndUserSuccessful { get; set; }
        public string CreateProviderEndUserResponse { get; set; }
        public bool IsCreateCardDetailsInitiated { get; set; }
        public bool IsCreateCardDetailsSuccessful { get; set; }
        public string CreateCardDetailsResponse { get; set; }

        public bool IsCreateCardInitiated { get; set; }
        public bool IsCreateCardSuccessful { get; set; }
        public string CreateCardResponse { get; set; }
        public string TransactionLimit { get; set; }


        public static Result<CardRequest> CreateCardRequest(Guid cardRequestId, Guid customerId, string firstName,
            string lastName,  string email, string address, DateTime dateOfBirth, string countryIso, CustomerType customerType,
            string phoneNumber, string currencyCode, Guid productId, CardType cardType, string postalCode,
            Guid accountId, string city, string middleName, CardDesignType design, string cardDeliveryName, string transactionLimit)
        {
            if (cardRequestId == default)
                return Result.Fail<CardRequest>($"{nameof(cardRequestId)} is invalid");

            if (customerId == default)
                return Result.Fail<CardRequest>($"{nameof(customerId)} is invalid");

            if (firstName == default)
                return Result.Fail<CardRequest>($"{nameof(firstName)} is invalid");

            if (lastName == default)
                return Result.Fail<CardRequest>($"{nameof(lastName)} is invalid");

            if (email == default)
                return Result.Fail<CardRequest>($"{nameof(email)} is invalid");

            if (address == default)
                return Result.Fail<CardRequest>($"{nameof(address)} is invalid");

            if (dateOfBirth == default)
                return Result.Fail<CardRequest>($"{nameof(dateOfBirth)} is invalid");

            if (countryIso == default)
                return Result.Fail<CardRequest>($"{nameof(countryIso)} is invalid");

            if (customerType == default)
                return Result.Fail<CardRequest>($"{nameof(customerType)} is invalid");

            if (currencyCode == default)
                return Result.Fail<CardRequest>($"{nameof(currencyCode)} is invalid");

            if (productId == default)
                return Result.Fail<CardRequest>($"{nameof(productId)} is invalid");

            if (postalCode == default)
                return Result.Fail<CardRequest>($"{nameof(postalCode)} is invalid");

            if (accountId == default)
                return Result.Fail<CardRequest>($"{nameof(accountId)} is invalid");
            
            if (city == default)
                return Result.Fail<CardRequest>($"{nameof(city)} is invalid");
            
            /*if (middleName == default)
                return Result.Fail<CardRequest>($"{nameof(middleName)} is invalid"); */
            
            if (design == default)
                return Result.Fail<CardRequest>($"{nameof(design)} is invalid");
            
            if (cardDeliveryName == default)
                return Result.Fail<CardRequest>($"{nameof(cardDeliveryName)} is invalid");
            
            if (dateOfBirth.AddYears(1) > DateTime.Now)
                return Result.Fail<CardRequest>($"{nameof(DateOfBirth)} is invalid");
            
            return Result.Ok(new CardRequest(cardRequestId, customerId, firstName, lastName, email, address,
                dateOfBirth, countryIso, customerType, CardRequestStatus.Pending, phoneNumber.Trim(), currencyCode, productId,
                cardType, postalCode, accountId, city, middleName, design, cardDeliveryName, transactionLimit));
        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}