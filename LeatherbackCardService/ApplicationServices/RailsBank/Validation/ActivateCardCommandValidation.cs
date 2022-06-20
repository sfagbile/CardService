using ApplicationServices.RailsBank.Cards.Command;
using FluentValidation;

namespace Leatherback.PaymentGateway.ApplicationServices.RailsBank.Validation
{
    public class ActivateCardCommandValidation: AbstractValidator<ActivateCardCommand>
    {
        public ActivateCardCommandValidation()
        {
            RuleFor(x => x.CardId).NotNull().WithMessage("Card id can not be null");
            RuleFor(x => x.CardId).Must(customerId => customerId != default).WithMessage("Card id is invalid");
        }
    }
}