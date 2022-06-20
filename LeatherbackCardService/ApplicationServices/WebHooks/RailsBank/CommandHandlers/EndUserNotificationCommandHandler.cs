using System.Linq;
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
    public class EndUserNotificationCommandHandler : IRequestHandler<EndUserNotificationCommand, Result>
    {
        private readonly ILogger<EndUserNotificationCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly ICardServiceDbContext _dbContext;

        public EndUserNotificationCommandHandler(ILogger<EndUserNotificationCommandHandler> logger, IMediator mediator,
            ICardServiceDbContext dbContext)
        {
            _logger = logger;
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(EndUserNotificationCommand request, CancellationToken cancellationToken)
        {
            Result result = null;
            _logger.LogInformation($"{nameof(EndUserNotificationCommandHandler)} request :: {request.ToJson()}");

            var providerEndUser = await _dbContext.ProviderEndUsers.Include(x => x.CardRequest)
                .FirstOrDefaultAsync(x => x.EndUserId == request.EndUserId,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (providerEndUser is null)
                return Result.Fail($"{nameof(request.EndUserId)} : {request.EndUserId} not found");

            result = request.Type switch
            {
                NotificationType.EntityFwQuarantine or NotificationType.EntityFwMissingData =>
                    await ProcessFailedCreateEndUser(cancellationToken, request, providerEndUser),
                NotificationType.EntityReadyToUse => await ProcessCompletedCreateEndUser(cancellationToken, request,
                    providerEndUser),
                _ => Result.Ok("NotImplemented")
            };

            _logger.LogInformation($"{nameof(EndUserNotificationCommandHandler)} response :: {request.ToJson()}");
            return result;
        }

        private async Task<Result> ProcessCompletedCreateEndUser(CancellationToken cancellationToken,
            EndUserNotificationCommand request, ProviderEndUser providerEndUser)
        {
            providerEndUser.Status = CardRequestStatus.Completed;
            _dbContext.ProviderEndUsers.Update(providerEndUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator
                .Send(
                    new UpdateCardRequestProcessCommand
                    {
                        Status = CardRequestStatus.Inprogress,
                        CardRequestId = providerEndUser.CardRequestId,
                        IsCreateCustomerInitiated = providerEndUser.CardRequest.IsCreateCustomerInitiated,
                        IsCreateCustomerSuccessful = providerEndUser.CardRequest.IsCreateCustomerSuccessful,
                        IsCreateProviderEndUserInitiated =
                            providerEndUser.CardRequest.IsCreateProviderEndUserInitiated,
                        IsCreateProviderEndUserSuccessful = true,
                        IsCreateCardDetailsInitiated = false,
                        IsCreateCardDetailsSuccessful = false,
                        IsCreateCardInitiated = false,
                        IsCreateCardSuccessful = false,
                        CreateProviderEndUserResponse = request.ToJson(),
                        ShouldPublish = true,
                    }, cancellationToken)
                .ConfigureAwait(false);

            return Result.Ok("Completed");
        }

        private async Task<Result> ProcessFailedCreateEndUser(CancellationToken cancellationToken,
            EndUserNotificationCommand request, ProviderEndUser providerEndUser)
        {
            providerEndUser.Status = CardRequestStatus.Failed;
            _dbContext.ProviderEndUsers.Update(providerEndUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _mediator
                .Send(
                    new UpdateCardRequestProcessCommand
                    {
                        Status = CardRequestStatus.Inprogress,
                        CardRequestId = providerEndUser.CardRequestId,
                        IsCreateCustomerInitiated = providerEndUser.CardRequest.IsCreateCustomerInitiated,
                        IsCreateCustomerSuccessful = providerEndUser.CardRequest.IsCreateCustomerSuccessful,
                        IsCreateProviderEndUserInitiated =
                            providerEndUser.CardRequest.IsCreateProviderEndUserInitiated,
                        IsCreateProviderEndUserSuccessful = false,
                        IsCreateCardDetailsInitiated = false,
                        IsCreateCardDetailsSuccessful = false,
                        IsCreateCardInitiated = false,
                        IsCreateCardSuccessful = false,
                        CreateProviderEndUserResponse = request.ToJson(),
                        ShouldPublish = true,
                    }, cancellationToken)
                .ConfigureAwait(false);

            return Result.Ok("Completed");
        }
    }
}