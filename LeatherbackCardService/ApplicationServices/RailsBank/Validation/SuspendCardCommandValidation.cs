using ApplicationServices.RailsBank.Command;
using FluentValidation;

namespace Leatherback.PaymentGateway.ApplicationServices.RailsBank.Validation
{
    public class SuspendCardCommandValidation: AbstractValidator<SuspendCardCommand>
    {
        public SuspendCardCommandValidation()
        {
            RuleFor(x => x.Reason).NotNull().WithMessage("Customer id can not be null or empty");
            RuleFor(x => x.CardId).NotNull().WithMessage("Card id can not be null or empty");
            RuleFor(x => x.CardId).Must(customerId => customerId != default)
                .WithMessage("Card id is invalid");
        }
    }
}