using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Customer.Commands;
using ApplicationServices.Customer.Model;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.CardDetailServices;
using Domain.Entities.Customers;
using Domain.Entities.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.Extensions;

namespace ApplicationServices.Customer.CommandHandlers
{
    public class CreateProviderEndUserCommandHandler : IRequestHandler<CreateProviderEndUserCommand,
        Result<CreateProviderEndUserResponseModel>>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IStrategyResolver<IProviderEndUserService> _providerEndUserStrategyProcessor;
        private readonly ILogger<CreateProviderEndUserCommandHandler> _logger;

        public CreateProviderEndUserCommandHandler(ICardServiceDbContext dbContext,
            ILogger<CreateProviderEndUserCommandHandler> logger,
            IStrategyResolver<IProviderEndUserService> providerEndUserStrategyProcessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _providerEndUserStrategyProcessor = providerEndUserStrategyProcessor;
        }

        public async Task<Result<CreateProviderEndUserResponseModel>> Handle(CreateProviderEndUserCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(CreateProviderEndUserCommandHandler)} :: Request: {request.ToJson()}");

            var providerEndUser = await GetProviderEndUser(request, cancellationToken);

            if (providerEndUser?.EndUserId != null && providerEndUser.Status != CardRequestStatus.Failed)
            {
                providerEndUser.CardRequestId = request.CardRequestId;
                _dbContext.ProviderEndUsers.Update(providerEndUser);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var userResponseModel = new CreateProviderEndUserResponseModel
                {
                    Status = providerEndUser.Status switch
                    {
                        CardRequestStatus.Completed => RequestStatus.Completed,
                        CardRequestStatus.Inprogress => RequestStatus.Inprogress,
                    },
                    EndUserId = providerEndUser.EndUserId,
                };
                return Result.Ok(userResponseModel);
            }

            var endUserValueTuple = await PersistProviderEndUser(request, cancellationToken, providerEndUser);

            if (endUserValueTuple.Item2.IsSuccess is false)
            {
                _logger.LogInformation(
                    $"{nameof(CreateProviderEndUserCommandHandler)} :: Response: {endUserValueTuple.Item2.ToJson()}");
                return endUserValueTuple.Item2;
            }

            providerEndUser = endUserValueTuple.Item1;

            var providerEndUserService =
                _providerEndUserStrategyProcessor.GetService(providerEndUser?.CardProvider.Name);

            var createEndUserResult = providerEndUser?.Customer.CustomerType switch
            {
                CustomerType.Individual => await providerEndUserService.CreateIndividualEndUser(
                    providerEndUser.Customer, cancellationToken),
                CustomerType.Company => await providerEndUserService.CreateCompanyEndUser(providerEndUser.Customer,
                    cancellationToken),
            };

            var providerEndUserResponse = createEndUserResult?.Value;
            providerEndUser.CardRequestId = request.CardRequestId;

            if (createEndUserResult?.IsSuccess is false)
            {
                _logger.LogInformation(
                    $"{nameof(CreateProviderEndUserCommandHandler)} :: Response: {providerEndUserResponse.ToJson()}");

                await UpdateProviderEndUser(request, cancellationToken, providerEndUser);
                return Result.Fail(providerEndUserResponse, providerEndUserResponse.Message, "");
            }

            if (!string.IsNullOrWhiteSpace(createEndUserResult?.Value.EndUserId))
                providerEndUser.EndUserId = createEndUserResult.Value.EndUserId;

            providerEndUser.Status = providerEndUserService.HasWebHook
                ? CardRequestStatus.Inprogress
                : CardRequestStatus.Completed;

            await UpdateProviderEndUser(request, cancellationToken, providerEndUser);

            _logger.LogInformation(
                $"{nameof(CreateProviderEndUserCommandHandler)} :: Response: {providerEndUserResponse.ToJson()}");
            
            return Result.Ok(providerEndUserResponse);
        }

        private async Task UpdateProviderEndUser(CreateProviderEndUserCommand request,
            CancellationToken cancellationToken, ProviderEndUser providerEndUser)
        {
            _dbContext.ProviderEndUsers.Update(providerEndUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<ProviderEndUser> GetProviderEndUser(CreateProviderEndUserCommand request,
            CancellationToken cancellationToken)
        {
            var providerEndUser = await _dbContext.ProviderEndUsers.Include(x => x.CardProvider)
                .Include(x => x.Customer)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(
                    x => x.CardProviderId == request.CardProviderId && x.CustomerId == request.CustomerId,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return providerEndUser;
        }

        private async Task<(ProviderEndUser, Result<CreateProviderEndUserResponseModel>)> PersistProviderEndUser(
            CreateProviderEndUserCommand request, CancellationToken cancellationToken, ProviderEndUser providerEndUser)
        {
            if (providerEndUser != null)
                return (providerEndUser, Result.Ok(new CreateProviderEndUserResponseModel { }));

            var providerEndUserResult =
                ProviderEndUser.CreateInstance(request.CustomerId, "", request.CardProviderId, request.CardRequestId);

            if (!providerEndUserResult.IsSuccess)
                return (default,
                    Result.Fail(
                        new CreateProviderEndUserResponseModel
                        {
                            Status = RequestStatus.Failed,
                            Message = providerEndUserResult.Error,
                            ProviderResponse = providerEndUserResult.ToJson()
                        }, providerEndUserResult.Error, ""));
            providerEndUser = providerEndUserResult.Value;

            await _dbContext.ProviderEndUsers.AddAsync(providerEndUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            providerEndUser = await GetProviderEndUser(request, cancellationToken);

            return (providerEndUser, Result.Ok(new CreateProviderEndUserResponseModel { }));
        }
    }
}