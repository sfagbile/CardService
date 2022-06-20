using ApplicationServices.RailsBank.Cards.Command;
using FluentValidation;

namespace Leatherback.PaymentGateway.ApplicationServices.RailsBank.Validation
{
    public class IssueVirtualCardCommandValidation: AbstractValidator<IssueVirtualCardCommand>
    {
        public IssueVirtualCardCommandValidation()
        {
            RuleFor(x => x.LedgerId).NotNull().WithMessage("Ledger id can not be null or empty");
            RuleFor(x => x.CustomerId).Must(customerId => customerId != default).WithMessage("Customer id is invalid");
        }
    }
}