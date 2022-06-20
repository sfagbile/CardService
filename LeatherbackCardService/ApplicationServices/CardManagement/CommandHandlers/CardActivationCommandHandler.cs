using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.CardManagement.Commands;
using ApplicationServices.CardManagement.Models;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.Card;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.Utility;

namespace ApplicationServices.CardManagement.CommandHandlers
{
    public class
        CardActivationCommandHandler : IRequestHandler<CardActivationCommand, Result<CardActivationResponseModel>>
    {
        private readonly ILogger<CardActivationCommandHandler> _logger;
        private readonly ICardServiceDbContext _dbContext;
        private readonly IStrategyResolver<ICardManagementService> _cardManagementStrategyProcessor;
        private readonly ICardPinService _cardPinService;

        public CardActivationCommandHandler(IStrategyResolver<ICardManagementService> cardManagementStrategyProcessor,
            ICardServiceDbContext dbContext, ILogger<CardActivationCommandHandler> logger,
            ICardPinService cardPinService)
        {
            _cardManagementStrategyProcessor = cardManagementStrategyProcessor;
            _dbContext = dbContext;
            _logger = logger;
            _cardPinService = cardPinService;
        }

        public async Task<Result<CardActivationResponseModel>> Handle(CardActivationCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(CardActivationCommandHandler)} :: Request: {request.ToJson()}");

            Result<CardActivationResponseModel> result;
            var card = await _dbContext.Cards.Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.Customer)
                .Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.CardProvider)
                .FirstOrDefaultAsync(x => x.Id == request.CardId, cancellationToken: cancellationToken);

            if (card is null)
            {
                result = Result.Fail(
                    new CardActivationResponseModel
                    {
                        CardId = request.CardId,
                        CardActivationStatus = EnumUtility.GetEnumDescription(CardStatus.CardNotFound),
                        Status = RequestStatus.Failed,
                    }, EnumUtility.GetEnumDescription(CardStatus.CardNotFound), "");

                _logger.LogInformation($"{nameof(CardActivationCommandHandler)} :: {result.ToJson()}");
                return result;
            }

            var saveCardPinResult = await _cardPinService.SaveCardPin(request.Pin, card, cancellationToken);
            if (saveCardPinResult.IsSuccess is false)
            {
                result = Result.Fail(
                    new CardActivationResponseModel
                    {
                        CardId = request.CardId,
                        CardActivationStatus = EnumUtility.GetEnumDescription(CardStatus.CardFailedToActivate),
                        Status = RequestStatus.Failed,
                        ProviderResponse = saveCardPinResult.ToJson()
                    }, saveCardPinResult.Value, "");

                _logger.LogInformation($"{nameof(CardActivationCommandHandler)} :: {result.ToJson()}");
                return result;
            }

            var cardManagementService =
                _cardManagementStrategyProcessor.GetService(card.CardDetails.ProviderEndUser.CardProvider.Name);

            result = await cardManagementService.ActivateCard(new CardActivationRequestModel {Card = card},
                cancellationToken);

            if (result.IsSuccess && cardManagementService.HasWebHook == false)
            {
                card.CardStatus = CardStatus.CardActivated;
                card.LastModified = DateTime.Now;
                _dbContext.Cards.Update(card);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            _logger.LogInformation($"{nameof(CardActivationCommandHandler)} :: {result.ToJson()}");
            return result;
        }
    }
}