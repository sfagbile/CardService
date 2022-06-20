using ApplicationServices.Card.Query;
using FluentValidation;
using Shared.Validation;

namespace ApplicationServices.Card.Validation
{
    public class GetCardByCustomerIdValidation : AbstractValidator<GetCardByCustomerIdQuery>
    {
        public GetCardByCustomerIdValidation()
        {
            RuleFor(x => x.CustomerId).NotEmpty().Must(ValidationHelper.IsValidGuid)
                .WithMessage("Country must be provided");
        }
    }
}