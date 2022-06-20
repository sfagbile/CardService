using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Command;
using ApplicationServices.Interfaces;
using ApplicationServices.WebHooks.RailsBank.Commands;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using LeatherbackSharedLibrary.Messages.Card;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.InternalBusMessages;

namespace ApplicationServices.WebHooks.RailsBank.CommandHandlers
{
    public class CardNotificationCommandHandler : IRequestHandler<CardNotificationCommand, Result>
    {
        private readonly ILogger<CardNotificationCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly ICardServiceDbContext _dbContext;
        private readonly IMessagePublisher _messagePublisher;

        public CardNotificationCommandHandler(ILogger<CardNotificationCommandHandler> logger, IMediator mediator,
            ICardServiceDbContext dbContext, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _mediator = mediator;
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
        }

        public async Task<Result> Handle(CardNotificationCommand request, CancellationToken cancellationToken)
        {
            Result result = null;
            _logger.LogInformation($"{nameof(CardNotificationCommandHandler)} request :: {request.ToJson()}");

            var card = await _dbContext.Cards.Include(x => x.CardDetails)
                .ThenInclude(x => x.CardRequest)
                .FirstOrDefaultAsync(x => x.CardIdentifier == request.CardId, cancellationToken)
                .ConfigureAwait(false);

            if (card is null) return Result.Fail($"{nameof(request.CardId)} : {request.CardId} not found");

            result = request.Type switch
            {
                NotificationType.CardAwaitingActivation => await ProcessCardAwaitingActivationNotification(request,
                    cancellationToken, card),
                NotificationType.CardActivated => await UpdateCardStatus(cancellationToken, card, request),
                NotificationType.CardFailedToActivate => await UpdateCardStatus(cancellationToken, card, request),
                NotificationType.CardSuspended => await ProcessCardSuspensionNotification(request, card,
                    cancellationToken),
                NotificationType.CardFailedToSuspend => await UpdateCardStatus(cancellationToken, card, request),
                NotificationType.CardFailedToReplace => await UpdateCardStatus(cancellationToken, card, request),
                NotificationType.CardFailed => await ProcessCardFailedNotification(request, cancellationToken, card),
                NotificationType.CardClosed => await ProcessCardClosureNotification(request, card, cancellationToken),
                NotificationType.CardFailedToClose => await UpdateCardStatus(cancellationToken, card, request),
                _ => Result.Ok("NotImplemented")
            };

            _logger.LogInformation($"{nameof(CardNotificationCommandHandler)} response :: {result.ToJson()}");
            return result;
        }

        private async Task<Result> ProcessCardFailedNotification(CardNotificationCommand request,
            CancellationToken cancellationToken, Domain.Entities.Cards.Card card)
        {
            card.CardStatus = CardStatus.CardIssuanceInProgress;

            _dbContext.Cards.Update(card);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await _mediator
                .Send(
                    new UpdateCardRequestProcessCommand
                    {
                        Status = CardRequestStatus.Failed,
                        CardRequestId = card.CardDetails.CardRequestId,
                        IsCreateCustomerInitiated = card.CardDetails.CardRequest.IsCreateCustomerInitiated,
                        IsCreateCustomerSuccessful = card.CardDetails.CardRequest.IsCreateCustomerSuccessful,
                        IsCreateCardDetailsInitiated = card.CardDetails.CardRequest.IsCreateCardDetailsInitiated,
                        IsCreateCardDetailsSuccessful = card.CardDetails.CardRequest.IsCreateCardDetailsSuccessful,
                        IsCreateProviderEndUserInitiated =
                            card.CardDetails.CardRequest.IsCreateProviderEndUserInitiated,
                        IsCreateProviderEndUserSuccessful =
                            card.CardDetails.CardRequest.IsCreateProviderEndUserSuccessful,
                        IsCreateCardInitiated = true,
                        IsCreateCardSuccessful = false,
                        CreateCardResponse = request.ToJson(),
                        ShouldPublish = true
                    }, cancellationToken)
                .ConfigureAwait(false);

            return Result.Ok("Completed");
        }

        private async Task<Result> ProcessCardAwaitingActivationNotification(CardNotificationCommand request,
            CancellationToken cancellationToken, Domain.Entities.Cards.Card card)
        {
            card.CardStatus = CardStatus.CardAwaitingActivation;
            _dbContext.Cards.Update(card);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await _messagePublisher.Publish(new RailsBankGetCardDetailsMessage {CardId = card.Id})
                .ConfigureAwait(false);

            await _mediator
                .Send(
                    new UpdateCardRequestProcessCommand
                    {
                        Status = CardRequestStatus.Completed,
                        CardRequestId = card.CardDetails.CardRequestId,
                        IsCreateCustomerInitiated = card.CardDetails.CardRequest.IsCreateCustomerInitiated,
                        IsCreateCustomerSuccessful = card.CardDetails.CardRequest.IsCreateCustomerSuccessful,
                        IsCreateCardDetailsInitiated = card.CardDetails.CardRequest.IsCreateCardDetailsInitiated,
                        IsCreateCardDetailsSuccessful = card.CardDetails.CardRequest.IsCreateCardDetailsSuccessful,
                        IsCreateProviderEndUserInitiated =
                            card.CardDetails.CardRequest.IsCreateProviderEndUserInitiated,
                        IsCreateProviderEndUserSuccessful =
                            card.CardDetails.CardRequest.IsCreateProviderEndUserSuccessful,
                        IsCreateCardInitiated = true,
                        IsCreateCardSuccessful = true,
                        CreateCardResponse = request.ToJson(),
                        ShouldPublish = true
                    }, cancellationToken)
                .ConfigureAwait(false);

            return Result.Ok("Completed");
        }


        private async Task<Result> ProcessCardActivationNotification(CardNotificationCommand request,
            CancellationToken cancellationToken, Domain.Entities.Cards.Card card)
        {
            card.CardStatus = CardStatus.CardAwaitingActivation;
            _dbContext.Cards.Update(card);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await _messagePublisher.Publish(new RailsBankGetCardDetailsMessage {CardId = card.Id})
                .ConfigureAwait(false);

            return Result.Ok("Completed");
        }

        private async Task<Result> ProcessCardSuspensionNotification(CardNotificationCommand request,
            Domain.Entities.Cards.Card card,
            CancellationToken cancellationToken)
        {
            await UpdateCardStatus(cancellationToken, card, request);
            await _messagePublisher.Publish(new CardSuspensionMessage
            {
                CardId = card.Id,
                CardSuspensionStatus = Enum.GetName(CardStatus.CardSuspended)
            });
            return Result.Ok();
        }

        private async Task<Result> ProcessCardClosureNotification(CardNotificationCommand request,
            Domain.Entities.Cards.Card card,
            CancellationToken cancellationToken)
        {
            await UpdateCardStatus(cancellationToken, card, request);
            await _messagePublisher.Publish(new CardClosureMessage
            {
                CardId = card.Id,
                CardClosureStatus = Enum.GetName(CardStatus.CardClosed)
            });
            return Result.Ok();
        }

        private async Task<Result> UpdateCardStatus(CancellationToken cancellationToken,
            Domain.Entities.Cards.Card card, CardNotificationCommand request)
        {
            card.CardStatus = request.Type switch
            {
                NotificationType.CardActivated => CardStatus.CardActivated,
                NotificationType.CardFailedToActivate => CardStatus.CardFailedToActivate,
                NotificationType.CardSuspended => CardStatus.CardSuspended,
                NotificationType.CardFailedToSuspend => CardStatus.CardFailedToSuspend,
                NotificationType.CardFailedToReplace => CardStatus.CardFailedToReplace,
                NotificationType.CardClosed => CardStatus.CardClosed,
                NotificationType.CardFailedToClose => CardStatus.CardFailedToClose,
                _ => card.CardStatus
            };

            _dbContext.Cards.Update(card);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await PublishCardStatusToTransactionService(card, request);

            return Result.Ok("Completed");
        }

        private async Task PublishCardStatusToTransactionService(Domain.Entities.Cards.Card card,
            CardNotificationCommand request)
        {

            if (string.Equals(request.Type, NotificationType.CardClosed, StringComparison.Ordinal))
            {
                await _messagePublisher.Publish(new CardClosureMessage
                {
                    CardId = card.Id,
                    CardClosureStatus = request.Type
                });
            }
            else if (string.Equals(request.Type, NotificationType.CardSuspended, StringComparison.Ordinal))
            {
                await _messagePublisher.Publish(new CardSuspensionMessage
                {
                    CardId = card.Id,
                    CardSuspensionStatus = request.Type
                });
            }
            else 
            {
                await _messagePublisher.Publish(new CardSuspensionMessage
                {
                    CardId = card.Id,
                    CardSuspensionStatus = request.Type
                });
            }
        }
    }
}