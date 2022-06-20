using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Command;
using ApplicationServices.Card.Model;
using Domain.Entities.Cards;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;

namespace ApplicationServices.Card.CommandHandler
{
    public class
        CreatCardRequestCommandHandler : IRequestHandler<CreateCardRequestCommand, Result<CardRequestResponseViewModel>>
    {
        private readonly ILogger<CreatCardRequestCommandHandler> _logger;
        private readonly ICardServiceDbContext _dbContext;

        public CreatCardRequestCommandHandler(ILogger<CreatCardRequestCommandHandler> logger,
            ICardServiceDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<CardRequestResponseViewModel>> Handle(CreateCardRequestCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreatCardRequestCommandHandler: Request - {request.ToJson()}");

            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Name == request.Product,
                cancellationToken: cancellationToken);

            var validationResult = await ValidateRequest(request, product, cancellationToken);
            if (validationResult.IsSuccess is false) return validationResult;

            var cardRequestResult = CardRequest.CreateCardRequest(Guid.NewGuid(), request.CustomerId, request.FirstName,
                request.LastName, request.Email, request.Address, request.DateOfBirth, request.CountryIso,
                request.CustomerType, request.PhoneNumber, request.CurrencyCode, product.Id, request.CardType,
                request.PostalCode, request.AccountId, request.City, request.MiddleName, request.Design,
                request.CardDeliveryName, request.CardLimit.ToJson());

            if (cardRequestResult.IsSuccess is false)
            {
                _logger.LogInformation($"CreatCardRequestCommandHandler: {cardRequestResult.Error}");

                return Result.Fail<CardRequestResponseViewModel>($"{cardRequestResult.Error}");
            }

            var cardRequest = cardRequestResult.Value;
            cardRequest.Status = CardRequestStatus.Pending;

            await _dbContext.CardRequests.AddAsync(cardRequest, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(new CardRequestResponseViewModel {CardRequestId = cardRequest.Id});
        }

        private async Task<Result<CardRequestResponseViewModel>> ValidateRequest(CreateCardRequestCommand request,
            Product product, CancellationToken cancellationToken)
        {

            if (await _dbContext.CardProviderCurrencies.Include(x => x.Currency)
                .AnyAsync(
                    x => x.CardType == request.CardType && x.Currency.Code == request.CurrencyCode &&
                         x.CustomerType == request.CustomerType, cancellationToken) != true)
            {
                _logger.LogInformation(
                    $"CreatCardRequestCommandHandler: CardProviderCurrency not configured for {request.CardType}, {request.CurrencyCode} and {request.CustomerType} ");

                return Result.Fail<CardRequestResponseViewModel>(
                    $"CardProviderCurrency not configured for {request.CardType}, {request.CurrencyCode} and {request.CustomerType}");
            }

            //check if card exist 
            var card = await _dbContext.Cards.Include(x => x.CardDetails)
                .ThenInclude(x => x.Currency)
                .Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.Customer)
                .FirstOrDefaultAsync(
                    x => x.CardDetails.Currency.Code == request.CurrencyCode &&
                         x.CardDetails.ProviderEndUser.CustomerId == request.CustomerId &&
                         x.CardDetails.CardType == request.CardType &&
                         x.CardDetails.ProviderEndUser.Customer.CustomerType == request.CustomerType &&
                         (x.CardStatus == CardStatus.CardActivated ||
                          x.CardStatus == CardStatus.CardAwaitingActivation ||
                          x.CardStatus == CardStatus.CardActivationInProgress), cancellationToken: cancellationToken);

            if (card is not null)
                return Result.Fail<CardRequestResponseViewModel>(
                    $"Already have a card. Card Status: {Enum.GetName(card.CardStatus)}");

            return Result.Ok(new CardRequestResponseViewModel());
        }
    }
}