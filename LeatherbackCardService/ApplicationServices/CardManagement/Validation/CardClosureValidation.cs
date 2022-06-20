using System;
using ApplicationServices.CardManagement.Commands;
using FluentValidation;

namespace ApplicationServices.CardManagement.Validation
{
    public class CardClosureValidation : AbstractValidator<CardClosureCommand>
    {
        public CardClosureValidation()
        { 
            RuleFor(x => x.CardId)
                .NotEmpty().WithMessage("{PropertyName} must be provided")
                .Must(BeAValidGuid)
                .WithMessage("Invalid {PropertyName}");
            
            RuleFor(x => x.Reason).NotEmpty().WithMessage("{PropertyName} must be provided");
        }
        
        private bool BeAValidGuid(Guid val) => val != default;
    }
}