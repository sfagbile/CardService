using ApplicationServices.RailsBank.EndUsers.Query;
using FluentValidation;

namespace Leatherback.PaymentGateway.ApplicationServices.RailsBank.Validation
{
    public class GetRailsBankEndUserByCustomerIdQueryValidation: AbstractValidator<GetRailsBankEndUserByCustomerIdQuery>
    {
        public GetRailsBankEndUserByCustomerIdQueryValidation()
        {
            RuleFor(x => x.CustomerId).NotNull().WithMessage("Customer id can not be null or empty");
            RuleFor(x => x.CustomerId).Must(customerId => customerId != default)
                .WithMessage("Customer id is invalid");
        }
    }
}