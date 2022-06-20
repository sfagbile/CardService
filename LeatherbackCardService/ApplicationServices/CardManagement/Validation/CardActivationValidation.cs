using System;
using ApplicationServices.CardManagement.Commands;
using FluentValidation;

namespace ApplicationServices.CardManagement.Validation
{
    public class CardActivationValidation : AbstractValidator<CardActivationCommand>
    {
        public CardActivationValidation()
        {
            RuleFor(x => x.CardId)
                .NotEmpty().
                 WithMessage("{PropertyName} must be provided")
                .Must(BeAValidGuid)
                .WithMessage("Invalid {PropertyName}");
            
            RuleFor(x => x.Pin).NotEmpty().WithMessage("{PropertyName} must be provided");
        }
        
        private bool BeAValidGuid(Guid val) => val != default;
    }
}