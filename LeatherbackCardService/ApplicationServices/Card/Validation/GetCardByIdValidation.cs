using ApplicationServices.Card.Query;
using FluentValidation;
using Shared.Validation;

namespace ApplicationServices.Card.Validation
{
    public class GetCardByIdValidation : AbstractValidator<GetCardQuery>
    {
        public GetCardByIdValidation()
        {
            RuleFor(x => x.CardId).NotEmpty().Must(ValidationHelper.IsValidGuid)
                .WithMessage("CardId must be provided");
        }
    }
}