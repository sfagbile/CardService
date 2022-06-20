using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Command;
using ApplicationServices.Interfaces;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.InternalBusMessages;

namespace ApplicationServices.Card.CommandHandler
{
    public class UpdateCardRequestCommandHandler : IRequestHandler<UpdateCardRequestProcessCommand, Result>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<UpdateCardRequestCommandHandler> _logger;

        public UpdateCardRequestCommandHandler(ICardServiceDbContext dbContext,
            IMessagePublisher messagePublisher, ILogger<UpdateCardRequestCommandHandler> logger)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task<Result> Handle(UpdateCardRequestProcessCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"{nameof(UpdateCardRequestCommandHandler)} :: Request: {request.ToJson()}");

            var cardRequest = await _dbContext.CardRequests.FirstOrDefaultAsync(x => x.Id == request.CardRequestId,
                cancellationToken: cancellationToken);

            if (cardRequest is null)
            {
                _logger.LogInformation(
                    $"{nameof(UpdateCardRequestCommandHandler)} :: Invalid card request Id, {request.ToJson()}");
                return Result.Fail("Invalid card request Id");
            }

            cardRequest.IsCreateCustomerInitiated = request.IsCreateCustomerInitiated;
            cardRequest.IsCreateCustomerSuccessful = request.IsCreateCustomerSuccessful;
            
            cardRequest.IsCreateProviderEndUserInitiated = request.IsCreateProviderEndUserInitiated;
            cardRequest.IsCreateProviderEndUserSuccessful = request.IsCreateProviderEndUserSuccessful;

            cardRequest.IsCreateCardDetailsInitiated = request.IsCreateCardDetailsInitiated;
            cardRequest.IsCreateCardDetailsSuccessful = request.IsCreateCardDetailsSuccessful;
            

            cardRequest.IsCreateCardInitiated = request.IsCreateCardInitiated;
            cardRequest.IsCreateCardSuccessful = request.IsCreateCardSuccessful;
            cardRequest.Status = request.Status;

            if (!string.IsNullOrWhiteSpace(request.CreateCardResponse))
                cardRequest.CreateCardResponse = request.CreateCardResponse;

            if (!string.IsNullOrWhiteSpace(request.CreateCustomerResponse))
                cardRequest.CreateCustomerResponse = request.CreateCustomerResponse;

            if (!string.IsNullOrWhiteSpace(request.CreateCardDetailsResponse))
                cardRequest.CreateCardDetailsResponse = request.CreateCardDetailsResponse;

            if (!string.IsNullOrWhiteSpace(request.CreateProviderEndUserResponse))
                cardRequest.CreateProviderEndUserResponse = request.CreateProviderEndUserResponse;

            _dbContext.CardRequests.Update(cardRequest);
            await _dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation(
                $"{nameof(UpdateCardRequestCommandHandler)} :: CardRequest updated, {cardRequest.ToJson()}");

            if (request.ShouldPublish)
                await _messagePublisher.Publish(new CardRequestMessage
                {
                    CardRequestId = request.CardRequestId,
                    IsCreateCardInitiated = cardRequest.IsCreateCardInitiated,
                    IsCreateCardSuccessful = cardRequest.IsCreateCardSuccessful,
                    IsCreateCustomerInitiated = cardRequest.IsCreateCustomerInitiated,
                    IsCreateCustomerSuccessful = cardRequest.IsCreateCustomerSuccessful,
                    IsCreateCustomerCardDetailsInitiated = cardRequest.IsCreateCardDetailsInitiated,
                    IsCreateCustomerCardDetailsSuccessful = cardRequest.IsCreateCardDetailsSuccessful,
                    IsProviderEndUserInitiated = cardRequest.IsCreateProviderEndUserInitiated,
                    IsProviderEndUserSuccessful = cardRequest.IsCreateProviderEndUserSuccessful,
                });

            return Result.Ok();
        }
    }
}