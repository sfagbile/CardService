using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.CardManagement.Commands;
using ApplicationServices.CardManagement.Models;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.Card;
using Domain.Entities.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.Extensions;
using Shared.Utility;

namespace ApplicationServices.CardManagement.CommandHandlers
{
    public class
        ReactivateCardCommandHandler : IRequestHandler<ReactivateCardCommand, Result<CardActivationResponseModel>>
    {
        private readonly ILogger<ReactivateCardCommandHandler> _logger;
        private readonly ICardServiceDbContext _dbContext;
        private readonly IStrategyResolver<ICardManagementService> _cardManagementStrategyProcessor;
        private readonly ICardPinService _cardPinService;

        public ReactivateCardCommandHandler(ILogger<ReactivateCardCommandHandler> logger,
            IStrategyResolver<ICardManagementService> cardManagementStrategyProcessor, ICardServiceDbContext dbContext,
            ICardPinService cardPinService)
        {
            _logger = logger;
            _cardManagementStrategyProcessor = cardManagementStrategyProcessor;
            _dbContext = dbContext;
            _cardPinService = cardPinService;
        }


        public async Task<Result<CardActivationResponseModel>> Handle(ReactivateCardCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ReactivateCardCommandHandler)} :: Request: {request.ToJson()}");

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

                _logger.LogInformation($"{nameof(ReactivateCardCommandHandler)} :: {result.ToJson()}");
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

            _logger.LogInformation($"{nameof(ReactivateCardCommandHandler)} :: {result.ToJson()}");
            return result;
        }
    }
}