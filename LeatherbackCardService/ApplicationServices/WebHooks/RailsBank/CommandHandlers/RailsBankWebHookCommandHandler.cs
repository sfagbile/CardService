using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.WebHooks.RailsBank.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Shared.BaseResponse;

namespace ApplicationServices.WebHooks.RailsBank.CommandHandlers
{
    public class RailsBankWebHookCommandHandler : IRequestHandler<RailsBankWebHookCommand, Result>
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public RailsBankWebHookCommandHandler(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<Result> Handle(RailsBankWebHookCommand request, CancellationToken cancellationToken)
        {
            Result result = null;
            var secret = _configuration["RailsBank:Secret"];

            if (!request.Secret.Equals(secret, StringComparison.OrdinalIgnoreCase))
                return Result.Fail("Invalid secret");
            if (!string.IsNullOrWhiteSpace(request.EnduserId))
                result = await ProcessEndUserNotification(request, cancellationToken);
            else if (!string.IsNullOrWhiteSpace(request.LedgerId))
                result = await ProcessLedgerNotification(request, cancellationToken);
            else if (!string.IsNullOrWhiteSpace(request.CardId))
                result = await ProcessCardNotification(request, cancellationToken);
           else if (!string.IsNullOrWhiteSpace(request.TransactionId))
                result = await ProcessCardTransactionNotification(request, cancellationToken);
            else
                return Result.Ok("NotImplemented");
            return result;
        }

        private async Task<Result> ProcessCardNotification(RailsBankWebHookCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CardNotificationCommand
            {
                Owner = request.Owner,
                Type = request.Type,
                CreatedAt = request.CreatedAt,
                NotificationId = request.NotificationId,
                CardId = request.CardId
            }, cancellationToken).ConfigureAwait(false);
            return result;
        }
        
        
        private async Task<Result> ProcessCardTransactionNotification(RailsBankWebHookCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CardTransactionNotificationCommand
            {
                Owner = request.Owner,
                Type = request.Type,
                CreatedAt = request.CreatedAt,
                NotificationId = request.NotificationId,
                TransactionId = request.TransactionId,
                LedgerId = request.LedgerId
            }, cancellationToken).ConfigureAwait(false);
            return result;
        }

        private async Task<Result> ProcessLedgerNotification(RailsBankWebHookCommand request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new LedgerNotificationCommand
            {
                Owner = request.Owner,
                Type = request.Type,
                CreatedAt = request.CreatedAt,
                NotificationId = request.NotificationId,
                LedgerId = request.LedgerId,
            }, cancellationToken).ConfigureAwait(false);
            return result;
        }

        private async Task<Result> ProcessEndUserNotification(RailsBankWebHookCommand request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator
                .Send(
                    new EndUserNotificationCommand
                    {
                        Owner = request.Owner,
                        Type = request.Type,
                        CreatedAt = request.CreatedAt,
                        NotificationId = request.NotificationId,
                        EndUserId = request.EnduserId,
                        EndUserStatus = request.EnduserStatus
                    }, cancellationToken)
                .ConfigureAwait(false);
            return result;
        }
    }
}