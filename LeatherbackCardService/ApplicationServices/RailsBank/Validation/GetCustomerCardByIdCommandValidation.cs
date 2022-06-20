using ApplicationServices.RailsBank.Cards.Query;
using FluentValidation;

namespace Leatherback.PaymentGateway.ApplicationServices.RailsBank.Validation
{
    public class GetCustomerCardByIdCommandValidation: AbstractValidator<GetCustomerCardByIdCommand>
    {
        public GetCustomerCardByIdCommandValidation()
        {
            RuleFor(x => x.CardId).NotNull().WithMessage("Card id can not be null or empty");
            RuleFor(x => x.CardId).Must(customerId => customerId != default)
                .WithMessage("Card id is invalid");
        }
    }
}