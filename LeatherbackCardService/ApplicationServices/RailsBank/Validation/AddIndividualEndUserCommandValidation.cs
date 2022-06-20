using System.Collections.Generic;
using ApplicationServices.RailsBank.EndUsers.Command;
using FluentValidation;

namespace Leatherback.PaymentGateway.ApplicationServices.RailsBank.Validation
{
    public class AddIndividualEndUserCommandValidation: AbstractValidator<AddIndividualEndUserCommand>
    {
        public AddIndividualEndUserCommandValidation()
        {
            RuleFor(x => x.Person.FullName.FirstName).NotNull().WithMessage("Full name is required");
            RuleFor(x => x.Person.Email).NotNull().WithMessage("Email is required");
            RuleFor(x => x.Person.DateOfBirth).NotNull().WithMessage("Date of birth is required");
            RuleFor(x => x.Person.Address.AddressIsoCountry).NotNull().WithMessage("Iso address country is required");
            RuleFor(x => x.Person.Address.AddressPostalCode).NotNull().WithMessage("Address postal code is required");
            RuleFor(x => x.Person.Address.AddressRefinement).NotNull().WithMessage("Iso address refinement is required");
            RuleFor(x => x.Person.Address.AddressIsoCountry).NotNull().WithMessage("Iso address country is required");
            RuleFor(x => x.Person.Address.AddressCity).NotNull().WithMessage("Address city is required");
            RuleFor(x => x.Person.Address.AddressStreet).NotNull().WithMessage("Address street is required");
            RuleFor(x => x.Person.Nationality).NotNull().WithMessage("Nationality is required");
            RuleFor(x => x.Person.CountryOfResidence).NotNull().WithMessage("Country of residence is required");
        }
    }
}