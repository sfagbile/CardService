using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Command;
using ApplicationServices.WebHooks.RailsBank.Commands;
using Domain.Entities.Cards;
using Domain.Entities.Customers;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;

namespace ApplicationServices.WebHooks.RailsBank.CommandHandlers
{
    public class LedgerNotificationCommandHandler : IRequestHandler<LedgerNotificationCommand, Result>
    {
        private readonly ILogger<LedgerNotificationCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly ICardServiceDbContext _dbContext;

        public LedgerNotificationCommandHandler(ILogger<LedgerNotificationCommandHandler> logger, IMediator mediator,
            ICardServiceDbContext dbContext)
        {
            _logger = logger;
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(LedgerNotificationCommand request, CancellationToken cancellationToken)
        {
            Result result = null;
            _logger.LogInformation($"{nameof(LedgerNotificationCommandHandler)} request :: {request.ToJson()}");

            var customerCardDetail = await _dbContext.CardDetails
                .Include(x => x.CardRequest)
                .FirstOrDefaultAsync(x => x.ProviderLedgerId == request.LedgerId,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (customerCardDetail is null)
                return Result.Fail($"{nameof(request.LedgerId)} : {request.LedgerId} not found");

            result = request.Type switch
            {
                NotificationType.EntityFwMissingData => await ProcessFailedCreateLedger(request, cancellationToken,
                    customerCardDetail),
                NotificationType.EntityReadyToUse => await ProcessCompletedCreateLedger(request, cancellationToken,
                    customerCardDetail),
                _ => Result.Ok("NotImplemented")
            };

            _logger.LogInformation($"{nameof(LedgerNotificationCommandHandler)} response :: {request.ToJson()}");
            return result;
        }

        private async Task<Result> ProcessCompletedCreateLedger(LedgerNotificationCommand request,
            CancellationToken cancellationToken,
            CardDetail cardDetail)
        {
            cardDetail.Status = CardRequestStatus.Completed;
            _dbContext.CardDetails.Update(cardDetail);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await _mediator.Send(new UpdateCardRequestProcessCommand
            {
                Status = CardRequestStatus.Inprogress,
                CardRequestId = cardDetail.CardRequest.Id,
                IsCreateCustomerInitiated = cardDetail.CardRequest.IsCreateCustomerInitiated,
                IsCreateCustomerSuccessful = cardDetail.CardRequest.IsCreateCustomerSuccessful,
                IsCreateProviderEndUserInitiated = cardDetail.CardRequest.IsCreateProviderEndUserInitiated,
                IsCreateProviderEndUserSuccessful = cardDetail.CardRequest.IsCreateProviderEndUserSuccessful,
                IsCreateCardDetailsInitiated = true,
                IsCreateCardDetailsSuccessful = true,
                IsCreateCardInitiated = false,
                IsCreateCardSuccessful = false,
                CreateCardDetailsResponse = request.ToJson(),
                ShouldPublish = true,
            }, cancellationToken);

            return Result.Ok("Completed");
        }


        private async Task<Result> ProcessFailedCreateLedger(LedgerNotificationCommand request,
            CancellationToken cancellationToken,
            CardDetail cardDetail)
        {
            cardDetail.Status = CardRequestStatus.Failed;
            _dbContext.CardDetails.Update(cardDetail);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await _mediator.Send(new UpdateCardRequestProcessCommand
            {
                Status = CardRequestStatus.Failed,
                CardRequestId = cardDetail.CardRequest.Id,
                IsCreateCustomerInitiated = cardDetail.CardRequest.IsCreateCustomerInitiated,
                IsCreateCustomerSuccessful = cardDetail.CardRequest.IsCreateCustomerSuccessful,
                IsCreateProviderEndUserInitiated = cardDetail.CardRequest.IsCreateProviderEndUserInitiated,
                IsCreateProviderEndUserSuccessful = cardDetail.CardRequest.IsCreateProviderEndUserSuccessful,
                IsCreateCardDetailsInitiated = true,
                IsCreateCardDetailsSuccessful = false,
                IsCreateCardInitiated = false,
                IsCreateCardSuccessful = false,
                CreateCardDetailsResponse = request.ToJson(),
                ShouldPublish = true,
            }, cancellationToken);

            return Result.Ok("Completed");
        }
    }
}