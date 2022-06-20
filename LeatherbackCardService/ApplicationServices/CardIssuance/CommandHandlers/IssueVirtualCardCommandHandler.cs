using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using ApplicationServices.CardIssuance.Commands;
using ApplicationServices.CardIssuance.Model;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.Card;
using Domain.Entities.Cards;
using Domain.Entities.Enums;
using Domain.Entities.Transactions;
using Domain.Interfaces;
using Domain.ValueTypes;
using LeatherbackSharedLibrary.Caching.Extensions;
using LeatherbackSharedLibrary.Messages.Card;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.BaseResponse;

namespace ApplicationServices.CardIssuance.CommandHandlers
{
    public class
        IssueVirtualCardCommandHandler : IRequestHandler<IssueVirtualCardCommand, Result<VirtualCardRespondModel>>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IStrategyResolver<ICardIssuanceService> _cardIssuerStrategyProcessor;
        private readonly ILogger<IssueVirtualCardCommandHandler> _logger;
        private readonly IMessagePublisher _messagePublisher;

        public IssueVirtualCardCommandHandler(ICardServiceDbContext dbContext,
            IStrategyResolver<ICardIssuanceService> cardIssuerStrategyProcessor,
            ILogger<IssueVirtualCardCommandHandler> logger, IMessagePublisher messagePublisher)
        {
            _dbContext = dbContext;
            _cardIssuerStrategyProcessor = cardIssuerStrategyProcessor;
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        public async Task<Result<VirtualCardRespondModel>> Handle(IssueVirtualCardCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"{nameof(IssueVirtualCardCommandHandler)} :: Request: {request.ToJson()}");

            var cardDetail = await GetCardDetail(request, cancellationToken);

            var (isCardRequestValid, virtualCardRespond) =
                await IsCardRequestValid(request, cancellationToken, cardDetail);

            if (isCardRequestValid is false) return virtualCardRespond;

            var cardIssuerStrategyProcessor =
                _cardIssuerStrategyProcessor.GetService(cardDetail.ProviderEndUser.CardProvider.Name);

            var virtualCardResult =
                await cardIssuerStrategyProcessor.CreateVirtualCard(cardDetail, cancellationToken);

            if (virtualCardResult.IsSuccess is false)
                return Result.Fail(
                    new VirtualCardRespondModel
                    {
                        CustomerCardDetailId = cardDetail.Id,
                        Message = virtualCardResult.Error,
                        Status = RequestStatus.Failed,
                        ProviderResponse = virtualCardResult.Value.ProviderResponse
                    }, virtualCardResult.Error, "");

            var issueVirtualCardRespond = virtualCardResult.Value;

            var cardResult = CreateCardModel(issueVirtualCardRespond, cardDetail);
            var card = cardResult.Value;

 

            if (card is null)
                return Result.Fail(new VirtualCardRespondModel
                {
                    ProviderResponse = cardResult.ToJson(),
                }, cardResult.Message, "");

            await _dbContext.Cards.AddAsync(card, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            await AddTransactionLimit(cancellationToken, cardDetail, card.Id);

            if (cardIssuerStrategyProcessor.HasWebHook is false)
            {
                card.CardStatus = CardStatus.CardAwaitingActivation;
                await _messagePublisher.Publish(new CardSuspensionMessage //use activate card message model
                {
                    CardId = card.Id,
                    CardSuspensionStatus = Enum.GetName(CardStatus.CardAwaitingActivation)
                });
            }

            _logger.LogInformation(
                $"{nameof(IssueVirtualCardCommandHandler)} :: Card created: {card.ToJson()}");

            return Result.Ok(new VirtualCardRespondModel
            {
                Status = cardIssuerStrategyProcessor.HasWebHook ? RequestStatus.Inprogress : RequestStatus.Completed,
                CardId = issueVirtualCardRespond.CardProviderId,
                ProviderResponse = issueVirtualCardRespond.ProviderResponse,
                CustomerCardDetailId = cardDetail.ProviderEndUser.CustomerId
            });
        }

        private async Task AddTransactionLimit(CancellationToken cancellationToken, CardDetail cardDetail, Guid cardId)
        {
            if (string.IsNullOrWhiteSpace(cardDetail.CardRequest.TransactionLimit)) return;

            var transactionLimitViewModels =
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<CardLimitViewModel>>(cardDetail
                    .CardRequest.TransactionLimit);

            var transactionLimits = transactionLimitViewModels
                .Select(x => CardLimit.CreateInstance(
                    x.CardLimitTypeId, cardId, x.Amount, 0).Value);

            await _dbContext.CardLimits.AddRangeAsync(transactionLimits, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<CardDetail> GetCardDetail(IssueVirtualCardCommand request,
            CancellationToken cancellationToken)
        {
            var cardDetail =
                await _dbContext.CardDetails
                    .Include(x => x.ProviderEndUser)
                    .ThenInclude(x => x.CardProvider)
                    .Include(x => x.ProviderEndUser)
                    .ThenInclude(x => x.Customer)
                    .ThenInclude(x => x.Country)
                    .Include(x => x.CardRequest)
                    .FirstOrDefaultAsync(x => x.CardRequestId == request.CardRequestId,
                        cancellationToken: cancellationToken);
            return cardDetail;
        }

        private static Result<Domain.Entities.Cards.Card> CreateCardModel(
            IssueVirtualCardRespondModel issueVirtualCardRespond,
            CardDetail cardDetail)
        {
            var cardResult = Domain.Entities.Cards.Card.CreateCard(issueVirtualCardRespond.CardNumber,
                issueVirtualCardRespond.Cvv,
                new CardExpiration(issueVirtualCardRespond.Month, issueVirtualCardRespond.Year),
                cardDetail.CardRequest.CardDeliveryName, issueVirtualCardRespond.Status,
                issueVirtualCardRespond.CardStatusReason, issueVirtualCardRespond.CardCarrierType,
                cardDetail.Id, Enum.GetName(cardDetail.CardRequest.Design));

            var card = cardResult.Value;

            if (!cardResult.IsSuccess)
                return cardResult;

            card.SetCardIdentifier(issueVirtualCardRespond.CardIdentifier);
            card.SetCardQrCodeContent(issueVirtualCardRespond.CardQrCodeContent);
            card.MaskedPan = issueVirtualCardRespond.MaskedPan;
            card.ExpiryDate = issueVirtualCardRespond.ExpiryDate;
            card.CardStatus = CardStatus.CardIssuanceInProgress;

            return Result.Ok(card);
        }

        private async Task<(bool, Result<VirtualCardRespondModel>)> IsCardRequestValid(IssueVirtualCardCommand request,
            CancellationToken cancellationToken, CardDetail cardDetail)
        {
            Result<VirtualCardRespondModel> virtualCardRespond;
            if (cardDetail is null)
            {
                virtualCardRespond = Result.Fail(
                    new VirtualCardRespondModel
                    {
                        Message = $"{nameof(cardDetail.Id)} Id does not exists",
                        Status = RequestStatus.Failed,
                    }, $"{nameof(cardDetail.Id)} does not exists", "");

                return (false, virtualCardRespond);
            }

            var card = await _dbContext.Cards.FirstOrDefaultAsync(
                x => x.CardDetailId == cardDetail.Id && (x.CardStatus == CardStatus.CardActivated
                                                         || x.CardStatus == CardStatus.CardAwaitingActivation
                                                         || x.CardStatus == CardStatus.CardActivationInProgress),
                cancellationToken: cancellationToken);

            if (card is null) return (true, default);

            virtualCardRespond =
                Result.Fail(
                    new VirtualCardRespondModel
                    {
                        CustomerCardDetailId = cardDetail.Id,
                        Message = $"Already have a card. Card Status: {Enum.GetName(card.CardStatus)}",
                        CardId = card.CardIdentifier,
                        Status = RequestStatus.Failed,
                    }, $"Already have a card. Card Status:  {Enum.GetName(card.CardStatus)}", "");

            return (false, virtualCardRespond);
        }
    }
}