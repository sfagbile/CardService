using System;
using System.Linq;
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
    public class ApproveCardRequestCommandHandler : IRequestHandler<ApproveCardRequestCommand, Result>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<ApproveCardRequestCommandHandler> _logger;

        public ApproveCardRequestCommandHandler(ICardServiceDbContext dbContext, IMessagePublisher messagePublisher,
            ILogger<ApproveCardRequestCommandHandler> logger)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task<Result> Handle(ApproveCardRequestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"{nameof(ApproveCardRequestCommandHandler)} :: Request: {request.ToJson()}");

            var cardRequest = await _dbContext.CardRequests.FirstOrDefaultAsync(
                x => x.Id == request.CardRequestId, cancellationToken: cancellationToken).ConfigureAwait(false);

            if (cardRequest == null)
            {
                _logger.LogInformation(
                    $"{nameof(ApproveCardRequestCommandHandler)} :: Invalid: {nameof(request.CardRequestId)} ");
                return Result.Fail($"CardRequestId: {request.CardRequestId} does not exist.");
            }

            if (cardRequest.Status != CardRequestStatus.Pending)
            {
                _logger.LogInformation($"CardRequestId: {request.CardRequestId} is {Enum.GetName(cardRequest.Status)} ");
                return Result.Fail($"CardRequestId: {request.CardRequestId} is {Enum.GetName(cardRequest.Status)} ");
            }

            await _messagePublisher.Publish(new CardRequestMessage {CardRequestId = cardRequest.Id,});

            return Result.Ok("Card request is successfully initiated.");
        }
    }
}