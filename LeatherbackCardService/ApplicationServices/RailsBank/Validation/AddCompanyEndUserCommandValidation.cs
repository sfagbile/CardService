using System.Linq;
using ApplicationServices.RailsBank.EndUsers.Command;
using FluentValidation;

namespace Leatherback.PaymentGateway.ApplicationServices.RailsBank.Validation
{
    public class AddCompanyEndUserCommandValidation: AbstractValidator<AddCooperateEndUserCommand>
    {
        public AddCompanyEndUserCommandValidation()
        {
            RuleFor(x => x.Company.Name).NotNull().WithMessage("Company name is required");
            RuleFor(x => x.Company.TradingName).NotNull().WithMessage("Company trading name is required");
            RuleFor(x => x.Company.ListedOnStockExchange).NotNull().WithMessage("Listed on stock exchange is required");
            RuleFor(x => x.Company.RegistrationAddress.AddressIsoCountry).NotNull().WithMessage("Iso address country is required");
            RuleForEach(x => x.Company.Directors).Custom((list, context) =>
            {
                if(string.IsNullOrEmpty(list.Person.Email))
                    context.AddFailure("At least one director must have an email");
                if(string.IsNullOrEmpty(list.Person.Name))
                    context.AddFailure("At least one director must have a name");
                if(string.IsNullOrEmpty(list.Person.DateOfBirth))
                    context.AddFailure("At least one director must have an dob");
                if(!list.Person.CountryOfResidence.Any())
                    context.AddFailure("At least one director must have a country of residence");
            }).NotNull().WithMessage("Nationality is required");
        }
    }
}