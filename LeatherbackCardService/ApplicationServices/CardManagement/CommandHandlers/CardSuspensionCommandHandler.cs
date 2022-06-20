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
        CardSuspensionCommandHandler : IRequestHandler<CardSuspensionCommand, Result<CardSuspensionResponseModel>>
    {
        private readonly ILogger<CardSuspensionCommandHandler> _logger;
        private readonly ICardServiceDbContext _dbContext;
        private readonly IStrategyResolver<ICardManagementService> _cardManagementStrategyProcessor;
        private readonly ICardPinService _cardPinService;

        public CardSuspensionCommandHandler(ILogger<CardSuspensionCommandHandler> logger,
            ICardServiceDbContext dbContext, IStrategyResolver<ICardManagementService> cardManagementStrategyProcessor,
            ICardPinService cardPinService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cardManagementStrategyProcessor = cardManagementStrategyProcessor;
            _cardPinService = cardPinService;
        }

        public async Task<Result<CardSuspensionResponseModel>> Handle(CardSuspensionCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(CardSuspensionCommandHandler)} :: Request: {request.ToJson()}");
            Result<CardSuspensionResponseModel> result;
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
                    new CardSuspensionResponseModel
                    {
                        CardId = request.CardId,
                        CardSuspensionStatus = EnumUtility.GetEnumDescription(CardStatus.CardNotFound)
                    }, EnumUtility.GetEnumDescription(CardStatus.CardNotFound), "");
                _logger.LogInformation($"{nameof(CardSuspensionCommandHandler)} :: Request: {result.ToJson()}");
                return result;
            }

            /*var validateCardPinResult =
                await _cardPinService.ValidateCardPin(request.Pin, card, cancellationToken);
            if (validateCardPinResult.IsSuccess is false)
            {
                result = Result.Fail(
                    new CardSuspensionResponseModel
                    {
                        CardId = request.CardId,
                        CardSuspensionStatus = EnumUtility.GetEnumDescription(CardStatus.CardFailedToClose),
                        Status = RequestStatus.Failed,
                        ProviderResponse = validateCardPinResult.ToJson()
                    }, validateCardPinResult.Value, "");

                _logger.LogInformation($"{nameof(CardSuspensionCommandHandler)} :: {result.ToJson()}");
                return result;
            }*/

            var cardManagementService =
                _cardManagementStrategyProcessor.GetService(card.CardDetails.ProviderEndUser.CardProvider.Name);

            result = await cardManagementService.SuspendCard(
                new CardSuspensionRequestModel {Card = card, Reason = request.Reason},
                cancellationToken);

            if (result.IsSuccess && cardManagementService.HasWebHook == false)
            {
                card.CardStatus = CardStatus.CardSuspended;
                card.CardStatusReason = request.Reason;
                card.LastModified = DateTime.Now;
                _dbContext.Cards.Update(card);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            _logger.LogInformation($"{nameof(CardSuspensionCommandHandler)} :: Response: {result.ToJson()}");
            return result;
        }
    }
}