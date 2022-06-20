using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Command;
using ApplicationServices.Interfaces;
using Domain.Entities.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.Extensions;
using Shared.InternalBusMessages;

namespace ApplicationServices.Card.CommandHandler
{
    public class RejectCardRequestCommandHandler : IRequestHandler<RejectCardRequestCommand, Result>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<RejectCardRequestCommandHandler> _logger;

        public RejectCardRequestCommandHandler(ICardServiceDbContext dbContext, IMessagePublisher messagePublisher,
            ILogger<RejectCardRequestCommandHandler> logger)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task<Result> Handle(RejectCardRequestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"{nameof(RejectCardRequestCommand)} :: Request: {request.ToJson()}");

            var cardRequest = await _dbContext.CardRequests.FirstOrDefaultAsync(
                x => x.Id == request.CardRequestId, cancellationToken: cancellationToken).ConfigureAwait(false);

            if (cardRequest == null)
            {
                _logger.LogInformation(
                    $"{nameof(RejectCardRequestCommand)} :: Invalid: {nameof(request.CardRequestId)} ");
                return Result.Fail($"CardRequestId: {request.CardRequestId} does not exist.");
            }

            if (cardRequest.Status != CardRequestStatus.Pending)
            {
                _logger.LogInformation($"CardRequestId: {request.CardRequestId} is {Enum.GetName(cardRequest.Status)} ");
                return Result.Fail($"CardRequestId: {request.CardRequestId} is {Enum.GetName(cardRequest.Status)} ");
            }

            cardRequest.Status = CardRequestStatus.Rejected;
            cardRequest.CardRejectionReason = request.Reason;
            _dbContext.CardRequests.Update(cardRequest);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Result.Ok("Card request is successfully rejected.");
        }
    }
}