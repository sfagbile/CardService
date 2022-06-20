using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationServices.Card.Command;
using ApplicationServices.Card.Model;
using Domain.Entities.Enums;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ApplicationServices.Card.Validation
{
    public class CreatCardRequestValidation : AbstractValidator<CreateCardRequestCommand>
    {
        private readonly ICardServiceDbContext _dbContext;

        public CreatCardRequestValidation(ICardServiceDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.Address).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.City).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} must be provided");
            //RuleFor(x => x.Sex).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.AccountId).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.Product).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.CardType).NotEmpty().WithMessage("{PropertyName} must be provided");

            RuleFor(x => x.CountryIso).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.CurrencyCode).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.CardType).NotEmpty().WithMessage("{PropertyName} must be provided");


            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("{PropertyName} must be provided")
                .Must(BeAValidGuid)
                .WithMessage("Invalid {PropertyName}");


            RuleFor(x => x.CardLimit).Must(BeValidCardLimitId)
                .WithMessage("Invalid cardLimitTypeId");

            RuleFor(x => x.CustomerType).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("{PropertyName} must be provided");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("{PropertyName} must be provided");
            //RuleFor(x => x.MiddleName).NotEmpty().WithMessage("{PropertyName} must be provided");
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("{PropertyName} must be provided");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("{PropertyName} must be provided")
                .Must(BeAValidDate)
                .WithMessage("{PropertyName} must be date");

            RuleFor(x => x.CurrencyCode).NotEmpty().WithMessage("{PropertyName} must be provided")
                .Must(BeCurrencyCode)
                .WithMessage("Invalid {PropertyName}");

            RuleFor(x => x.CountryIso).NotEmpty().WithMessage("{PropertyName} must be provided")
                .Must(BeCountryIso)
                .WithMessage("Invalid {PropertyName}");
        }

        private bool BeCurrencyCode(string val)
        {
            return _dbContext.Currencies.Any(x => x.Code == val);
        }

        private bool BeCountryIso(string val)
        {
            return _dbContext.Countries.Any(x => x.Iso == val);
        }

        private static bool BeAValidDate(DateTime val)
        {
            return val != default;
        }

        private static bool BeAValidGuid(Guid val)
        {
            return val != default;
        }

        private bool BeValidCardLimitId(List<CardLimitViewModel> val)
        {
            if (val is not {Count: > 0})
                return true;

            var cardLimitTypeIds = val.Select(x => x.CardLimitTypeId).ToList();

            var  dbCardLimitTypeIds = _dbContext.CardLimitTypes
                .Select(x => x.Id).ToList();
            
          var matchedCardLimitTypeIds =  dbCardLimitTypeIds.FindAll(g1 => cardLimitTypeIds.All(g2 => g2 == g1));

            return matchedCardLimitTypeIds.Count > 0;
        }
    }
}