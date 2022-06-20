using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Customer.Commands;
using ApplicationServices.Customer.Model;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.CardDetailServices;
using Domain.Entities.Cards;
using Domain.Entities.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.Extensions;

namespace ApplicationServices.Customer.CommandHandlers
{
    public class
        CreateCardDetailCommandHandler : IRequestHandler<CreateCardDetailCommand,
            Result<CreateCardDetailResponseModel>>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IStrategyResolver<ICardDetailService> _cardDetailStrategyProcessor;
        private readonly ILogger<CreateCardDetailCommandHandler> _logger;

        public CreateCardDetailCommandHandler(ICardServiceDbContext dbContext,
            IStrategyResolver<ICardDetailService> cardDetailStrategyProcessor,
            ILogger<CreateCardDetailCommandHandler> logger)
        {
            _dbContext = dbContext;
            _cardDetailStrategyProcessor = cardDetailStrategyProcessor;
            _logger = logger;
        }

        public async Task<Result<CreateCardDetailResponseModel>> Handle(CreateCardDetailCommand request,
            CancellationToken cancellationToken)
        {
            Result<CreateCardDetailResponseModel> response = null;
            var createCardDetailResponse = new CreateCardDetailResponseModel();

            _logger.LogInformation($"{nameof(CreateCardDetailCommandHandler)} :: Request: {request.ToJson()}");

            var cardDetail = await GetCardDetail(request, cancellationToken);

            if (cardDetail != null)
                response = ValidateCardDetail(cardDetail);

            if (response is {IsSuccess: true})
            {
                cardDetail.CardRequestId = request.CardRequestId;
                _dbContext.CardDetails.Update(cardDetail);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return response;
            }

            var (cardDetails, result) = await PersistCardDetail(cardDetail, request, cancellationToken);

            if (result.IsSuccess == false)
                return result;

            cardDetail = cardDetails;

            var cardDetailStrategyProcessor =
                _cardDetailStrategyProcessor.GetService(cardDetail.ProviderEndUser.CardProvider.Name);

            var cardDetailsResult = request.CardType switch
            {
                CardType.Virtual => await cardDetailStrategyProcessor.CreatedVirtualCardDetails(cardDetail,
                    cancellationToken),
                CardType.Physical => await cardDetailStrategyProcessor.CreatePhysicalCardDetails(cardDetail,
                    cancellationToken)
            };
            var createLedgerResponseModel = cardDetailsResult.Value;

            if (cardDetailsResult.IsSuccess is false)
            {
                await UpdateCardDetails(cancellationToken, cardDetail, CardRequestStatus.Failed,
                    createLedgerResponseModel);

                response = Result.Fail(
                    new CreateCardDetailResponseModel
                    {
                        Message = cardDetailsResult.Value.Message,
                        ProviderResponse = cardDetailsResult.Value.ProviderResponse,
                        Status = RequestStatus.Failed
                    }, cardDetailsResult.Error, "");

                _logger.LogInformation(
                    $"{nameof(CreateCardDetailCommandHandler)} :: Response: {response.ToJson()}");

                return response;
            }

            await UpdateCardDetails(cancellationToken, cardDetail, CardRequestStatus.Inprogress,
                createLedgerResponseModel);

            createCardDetailResponse.ProviderResponse = cardDetailsResult.Value.ProviderResponse;
            createCardDetailResponse.CardDetailId = cardDetail.Id;
            createCardDetailResponse.Status = cardDetailStrategyProcessor.HasWebHook
                ? RequestStatus.Inprogress
                : RequestStatus.Completed;

            _logger.LogInformation(
                $"{nameof(CreateCardDetailCommandHandler)} :: Response: {createCardDetailResponse.ToJson()}");

            return Result.Ok(createCardDetailResponse);
        }

        private Result<CreateCardDetailResponseModel> ValidateCardDetail(CardDetail cardDetail)
        {
            Result<CreateCardDetailResponseModel> response;

            if (cardDetail.ProviderLedgerId == null)
                return Result.Fail(new CreateCardDetailResponseModel { }, "", "");

            if (cardDetail.Status == CardRequestStatus.Failed)
                return Result.Fail(new CreateCardDetailResponseModel { }, "", "");

            response = Result.Ok(new CreateCardDetailResponseModel
            {
                CardDetailId = cardDetail.Id,
                Status = cardDetail.Status switch
                {
                    CardRequestStatus.Completed => RequestStatus.Completed,
                    CardRequestStatus.Inprogress => RequestStatus.Completed //change once legderwebhook is  ready
                }
            });

            _logger.LogInformation(
                $"{nameof(CreateCardDetailCommandHandler)} :: Response: {response.ToJson()}");
            return response;
        }

        private async Task<CardProviderCurrency> GetCardProvider(CreateCardDetailCommand request,
            CancellationToken cancellationToken)
        {
            var cardProviderCurrency = await _dbContext.CardProviderCurrencies.FirstOrDefaultAsync(
                x => x.CurrencyId == request.CurrencyId &&
                     x.CustomerType == request.CustomerType && x.CardType == request.CardType && x.IsPrimary,
                cancellationToken: cancellationToken);

            return cardProviderCurrency;
        }

        private async Task UpdateCardDetails(CancellationToken cancellationToken, CardDetail cardDetail,
            CardRequestStatus status, CreateLedgerResponseModel createLedgerResponseModel)
        {
            cardDetail.Status = status;

            if (!string.IsNullOrWhiteSpace(createLedgerResponseModel.LedgerId))
                cardDetail.ProviderLedgerId = createLedgerResponseModel.LedgerId;

            _dbContext.CardDetails.Update(cardDetail);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task<(CardDetail, Result<CreateCardDetailResponseModel>)> PersistCardDetail(
            CardDetail cardDetail, CreateCardDetailCommand request, CancellationToken cancellationToken)
        {
            var cardProvider = await GetCardProvider(request, cancellationToken);

            if (cardProvider is null)
            {
                var result = Result.Fail(
                    new CreateCardDetailResponseModel
                    {
                        Message = $"No record found. {nameof(CardProviderCurrency)}", Status = RequestStatus.Failed
                    }, $"No record found. {nameof(CardProviderCurrency)}", "");

                _logger.LogInformation(
                    $"{nameof(CreateCardDetailCommandHandler)} :: Response: {result.ToJson()}");

                return (cardDetail, result);
            }

            if (cardDetail != null)
            {
                cardDetail.CardRequestId = request.CardRequestId;
                _dbContext.CardDetails.Update(cardDetail);
            }
            else
            {
                var providerEndUsers = await _dbContext.ProviderEndUsers.Include(x => x.CardRequest)
                    .FirstOrDefaultAsync(
                        x => x.CardRequestId == request.CardRequestId, cancellationToken: cancellationToken);

                var cardDetailResult = CardDetail.Create(providerEndUsers.Id, request.CurrencyId,
                    request.CardType, CardRequestStatus.Inprogress, request.CardRequestId, cardProvider.CardDesign,
                    cardProvider.CardProgramme, providerEndUsers.CardRequest.AccountId);

                if (cardDetailResult.IsSuccess is false)
                {
                    var result = Result.Fail(
                        new CreateCardDetailResponseModel
                        {
                            Message = $"{cardDetailResult.Error}", Status = RequestStatus.Failed
                        }, $"{cardDetailResult.Error}", "");

                    _logger.LogInformation(
                        $"{nameof(CreateCardDetailCommandHandler)} :: Response: {result.ToJson()}");

                    return (cardDetail, result);
                }

                cardDetail = cardDetailResult.Value;
                await _dbContext.CardDetails.AddAsync(cardDetail, cancellationToken).ConfigureAwait(false);
            }

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CreateCardDetailCommandHandler)} :: Error: {e}");
                return (cardDetail, Result.Fail(
                    new CreateCardDetailResponseModel
                    {
                        Message = $"{e.Message}", Status = RequestStatus.Failed
                    }, $"{e.Message}", ""));
            }

            cardDetail = await _dbContext.CardDetails
                .Include(x => x.ProviderEndUser)
                .ThenInclude(x => x.CardProvider)
                .Include(x => x.ProviderEndUser)
                .ThenInclude(x => x.Customer)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == cardDetail.Id, cancellationToken: cancellationToken);

            return (cardDetail, Result.Ok(new CreateCardDetailResponseModel()));
        }

        private async Task<CardDetail> GetCardDetail(CreateCardDetailCommand request,
            CancellationToken cancellationToken)
        {
            var cardDetail = await _dbContext.CardDetails
                .Include(x => x.ProviderEndUser)
                .ThenInclude(x => x.CardProvider)
                .Include(x => x.ProviderEndUser)
                .ThenInclude(x => x.Customer)
                .Include(x => x.Currency)
                .FirstOrDefaultAsync(
                    x => x.ProviderEndUser.CardRequestId == request.CardRequestId && x.CardType == request.CardType &&
                         x.CurrencyId == request.CurrencyId &&
                         x.ProviderEndUser.Customer.CustomerType == request.CustomerType,
                    cancellationToken: cancellationToken);

            return cardDetail;
        }
    }
}