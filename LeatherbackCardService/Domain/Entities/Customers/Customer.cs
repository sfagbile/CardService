using System;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using Shared;
using Shared.BaseResponse;

namespace Domain.Entities.Customers
{
    public class Customer : Entity<Guid>
    {

        private Customer(Guid id, string firstName, string lastName, string email,
            Sex sex, string phoneNumber, CustomerType customerType, Guid countryId, DateTime dateOfBirth,
            string address, string city, string postalCode, string middleName, Guid companyId)
        {
            Id = id;
            FirstName = firstName;
            Email = email;
            Sex = sex;
            PhoneNumber = phoneNumber;
            CustomerType = customerType;
            CountryId = countryId;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Address = address;
            City = city;
            PostalCode = postalCode;
            MiddleName = middleName;
            CompanyId = companyId;
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                if (CustomerType == CustomerType.Individual)
                {
                    return string.IsNullOrEmpty(MiddleName)
                        ? $"{FirstName} {LastName}"
                        : $"{FirstName} {MiddleName} {LastName}";
                }

                return FirstName;
            }
        }

        public Sex Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }

        public string PostalCode { get; set; }

        public DateTime DateOfBirth { get; set; }
        public CustomerType CustomerType { get; set; }
        
        public Guid CompanyId { get; set; }
        public string Address { get; set; }
        public Country Country { get; set; }
        public Guid CountryId { get; set; }

        public Guid? ProductId { get; set; }
        public Product Product { get; set; }


        public static Result<Customer> Create(Guid id, string firstName, string lastName, string email, Sex sex,
            string phoneNumber, CustomerType customerType, Guid countryId, DateTime dateOfBirth, string address,
            string city, string postalCode, string middleName, Guid companyId)
        {
            if (id == default)
                return Result.Fail<Customer>($"{nameof(id)} is invalid");

            if (firstName == default)
                return Result.Fail<Customer>($"{nameof(firstName)} is invalid");

            if (lastName == default)
                return Result.Fail<Customer>($"{nameof(lastName)} is invalid");

            if (email == default)
                return Result.Fail<Customer>($"{nameof(email)} is invalid");

           /* if (sex == default)
                return Result.Fail<Customer>($"{nameof(sex)} is invalid");*/

            if (phoneNumber == default)
                return Result.Fail<Customer>($"{nameof(phoneNumber)} is invalid");

            if (customerType == default)
                return Result.Fail<Customer>($"{nameof(customerType)} is invalid");

            if (countryId == default)
                return Result.Fail<Customer>($"{nameof(countryId)} is invalid");

            if (dateOfBirth == default)
                return Result.Fail<Customer>($"{nameof(dateOfBirth)} is invalid");


            if (address == default)
                return Result.Fail<Customer>($"{nameof(address)} is invalid");


            if (city == default)
                return Result.Fail<Customer>($"{nameof(city)} is invalid");

            if (postalCode == default)
                return Result.Fail<Customer>($"{nameof(postalCode)} is invalid");
            
            if (middleName == default)
                return Result.Fail<Customer>($"{nameof(postalCode)} is invalid");

            return Result.Ok(new Customer(id, firstName, lastName, email, sex, phoneNumber, customerType, countryId,
                dateOfBirth, address, city, postalCode,  middleName, companyId));
        }
        
        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}