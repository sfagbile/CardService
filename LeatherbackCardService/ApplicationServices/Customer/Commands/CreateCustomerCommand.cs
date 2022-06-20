using System;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Customer.Commands
{
    public class CreateCustomerCommand : IRequest<Result>
    {
        public Guid CustomerIdentity { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid CountryIso { get; set; }
        public Guid ProductId { get; set; }
        public Guid companyId { get; set; }
        public string PostalCode { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}