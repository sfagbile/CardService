using System;
using System.Threading.Tasks;
using ApplicationServices.Card.Command;
using ApplicationServices.CardIssuance.Commands;
using ApplicationServices.Customer.Commands;
using ApplicationServices.Interfaces;
using Domain.Entities.Cards;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using LeatherbackSharedLibrary.Messages.Card;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.InternalBusMessages;

namespace MessageBus.Consumers
{
    public class CardRequestSagaConsumer : IConsumer<CardRequestMessage>
    {
        private readonly ILogger<CardRequestSagaConsumer> _logger;
        private readonly IServiceProvider _provider;

        public CardRequestSagaConsumer(ILogger<CardRequestSagaConsumer> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public async Task Consume(ConsumeContext<CardRequestMessage> context)
        {
            try
            {
                var cardRequestMessage = context.Message;
                _logger.LogInformation($"CardRequestSagaConsumer : {cardRequestMessage.ToJson()}");

                using (var scope = _provider.CreateScope())
                {
                    var cardRequest = await scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>()
                        .CardRequests.FirstOrDefaultAsync(x => x.Id == cardRequestMessage.CardRequestId)
                        .ConfigureAwait(false);

                    if (cardRequest is null)
                    {
                        _logger.LogInformation(
                            $"CardRequestSagaConsumer: CardRequest not found - {cardRequestMessage.ToJson()}");
                        return;
                    }

                    ProcessCardRequest(cardRequestMessage, cardRequest)();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString(), "On Calling CardRequestSagaConsumer");
            }
        }

        private Action ProcessCardRequest(CardRequestMessage message, CardRequest cardRequest)
        {
            return (message.IsCreateCustomerInitiated, message.IsCreateCustomerSuccessful,
                    message.IsProviderEndUserInitiated, message.IsProviderEndUserSuccessful,
                    message.IsCreateCustomerCardDetailsInitiated, message.IsCreateCustomerCardDetailsSuccessful,
                    message.IsCreateCardInitiated, message.IsCreateCardSuccessful) switch
                {
                    (false, false, false, false, false, false, false, false) => async () =>
                    {
                        await CreateCustomer(cardRequest);
                    },

                    (true, false, false, false, false, false, false, false) => async () =>
                    {
                        await UpdateFailedCardRequest(cardRequest);
                    },
                    (true, true, false, false, false, false, false, false) => async () =>
                    {
                        await CreateProviderEndUser(cardRequest);
                    },

                    (true, true, true, false, false, false, false, false) => async () =>
                    {
                        await UpdateFailedCardRequest(cardRequest);
                    },

                    (true, true, true, true, false, false, false, false) => async () =>
                    {
                        await CreateCardDetails(cardRequest);
                    },

                    (true, true, true, true, true, false, false, false) => async () =>
                    {
                        await UpdateFailedCardRequest(cardRequest);
                    },

                    (true, true, true, true, true, true, false, false) => async () =>
                    {
                        await CreateCard(cardRequest);
                    },

                    (true, true, true, true, true, true, true, false) => async () =>
                    {
                        await UpdateFailedCardRequest(cardRequest);
                    },

                    (true, true, true, true, true, true, true, true) => async () =>
                    {
                        await CompleteSuccessfulCardRequest(cardRequest);
                    },

                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private async Task CompleteSuccessfulCardRequest(CardRequest cardRequest)
        {
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>();

                var card = await dbContext.Cards.Include(x => x.CardDetails)
                    .ThenInclude(x => x.CardRequest)
                    .FirstOrDefaultAsync(x => x.CardDetails.CardRequest.Id == cardRequest.Id && x.CardStatus == CardStatus.CardAwaitingActivation );

                var cardDetail = card.CardDetails;

                cardDetail.Status = CardRequestStatus.Completed;

                var cardRequestModel = cardDetail.CardRequest;
                cardRequestModel.Status = CardRequestStatus.Completed;

                dbContext.CardDetails.Update(cardDetail);
                dbContext.CardRequests.Update(cardRequestModel);

                await dbContext.SaveChangesAsync();

                var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();
                await publisher.Publish(new CreateCardMessage
                {
                    AccountId = cardRequest.AccountId,
                    CardId = card.Id,
                    CardType = Enum.GetName(card.CardDetails.CardType),
                    CardStatus = Enum.GetName(card.CardStatus),
                    CustomerId = cardRequest.CustomerId,
                    CurrencyCode = cardRequest.CurrencyCode,
                });
            }
        }

        private async Task CreateCard(CardRequest cardRequest)
        {
            using (var scope = _provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                try
                {
                    //check if it virtual or physical
                    var result = await mediator.Send(new IssueVirtualCardCommand {CardRequestId = cardRequest.Id});

                    var issueVirtualCardResponse = result.Value;
                    var status = issueVirtualCardResponse.Status switch
                    {
                        RequestStatus.Completed => CardRequestStatus.Inprogress,
                        RequestStatus.Failed => CardRequestStatus.Failed,
                        RequestStatus.Inprogress => CardRequestStatus.Inprogress,
                        _ => CardRequestStatus.Failed
                    };

                    await mediator.Send(new UpdateCardRequestProcessCommand
                    {
                        Status = status,
                        CardRequestId = cardRequest.Id,
                        IsCreateCustomerInitiated = cardRequest.IsCreateCustomerInitiated,
                        IsCreateCustomerSuccessful = cardRequest.IsCreateCustomerSuccessful,
                        IsCreateCardDetailsInitiated = cardRequest.IsCreateCardDetailsInitiated,
                        IsCreateCardDetailsSuccessful = cardRequest.IsCreateCardDetailsSuccessful,
                        IsCreateProviderEndUserInitiated = cardRequest.IsCreateProviderEndUserInitiated,
                        IsCreateProviderEndUserSuccessful = cardRequest.IsCreateProviderEndUserSuccessful,
                        IsCreateCardInitiated = true,
                        IsCreateCardSuccessful = issueVirtualCardResponse.Status == RequestStatus.Completed,
                        CreateCardResponse = result.ToJson(),
                        ShouldPublish = issueVirtualCardResponse.Status != RequestStatus.Inprogress,
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{nameof(CardRequestSagaConsumer)} :: {ex}");
                    await UpdateFailedCardRequest(cardRequest).ConfigureAwait(false);
                }
            }
        }

        private async Task CreateCardDetails(CardRequest cardRequest)
        {
            using (var scope = _provider.CreateScope())
            {
                try
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var currency = await GetCurrency(cardRequest, scope);

                    var result = await mediator.Send(new CreateCardDetailCommand
                    {
                        CardType = cardRequest.CardType,
                        CurrencyId = currency.Id,
                        CustomerId = cardRequest.CustomerId,
                        CustomerType = cardRequest.CustomerType,
                        CardRequestId = cardRequest.Id,
                        LeatherBackCardDesign = cardRequest.Design,
                    });

                    var customerCardDetailResponse = result.Value;
                    var status = customerCardDetailResponse.Status switch
                    {
                        RequestStatus.Completed => CardRequestStatus.Inprogress,
                        RequestStatus.Failed => CardRequestStatus.Failed,
                        RequestStatus.Inprogress => CardRequestStatus.Inprogress,
                        _ => CardRequestStatus.Failed
                    };

                    await mediator.Send(new UpdateCardRequestProcessCommand
                    {
                        Status = status,
                        CardRequestId = cardRequest.Id,
                        IsCreateCustomerInitiated = cardRequest.IsCreateCustomerInitiated,
                        IsCreateCustomerSuccessful = cardRequest.IsCreateCustomerSuccessful,
                        IsCreateProviderEndUserInitiated = cardRequest.IsCreateProviderEndUserInitiated,
                        IsCreateProviderEndUserSuccessful = cardRequest.IsCreateProviderEndUserSuccessful,
                        IsCreateCardDetailsInitiated = true,
                        IsCreateCardDetailsSuccessful = customerCardDetailResponse.Status == RequestStatus.Completed,
                        IsCreateCardInitiated = false,
                        IsCreateCardSuccessful = false,
                        CreateCardDetailsResponse = result.ToJson(),
                        ShouldPublish = customerCardDetailResponse.Status != RequestStatus.Inprogress,
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{nameof(CardRequestSagaConsumer)} :: {ex}");
                    await UpdateFailedCardRequest(cardRequest).ConfigureAwait(false);
                }
            }
        }

        private async Task CreateProviderEndUser(CardRequest cardRequest)
        {
            using (var scope = _provider.CreateScope())
            {
                try
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>();

                    var cardProviderCurrency = await dbContext.CardProviderCurrencies.Include(x => x.Currency)
                        .FirstOrDefaultAsync(x => x.Currency.Code == cardRequest.CurrencyCode);

                    var result = await mediator.Send(new CreateProviderEndUserCommand
                    {
                        CustomerId = cardRequest.CustomerId,
                        CardProviderId = cardProviderCurrency.CardProviderId,
                        CardRequestId = cardRequest.Id,
                    });

                    var customerCardDetailResponse = result.Value;
                    var status = customerCardDetailResponse.Status switch
                    {
                        RequestStatus.Completed => CardRequestStatus.Inprogress,
                        RequestStatus.Failed => CardRequestStatus.Failed,
                        RequestStatus.Inprogress => CardRequestStatus.Inprogress,
                        _ => CardRequestStatus.Failed
                    };

                    await mediator.Send(new UpdateCardRequestProcessCommand
                    {
                        Status = status,
                        CardRequestId = cardRequest.Id,
                        IsCreateCustomerInitiated = cardRequest.IsCreateCustomerInitiated,
                        IsCreateCustomerSuccessful = cardRequest.IsCreateCustomerSuccessful,
                        IsCreateProviderEndUserInitiated = true,
                        IsCreateProviderEndUserSuccessful =
                            customerCardDetailResponse.Status == RequestStatus.Completed,
                        IsCreateCardDetailsInitiated = false,
                        IsCreateCardDetailsSuccessful = false,
                        IsCreateCardInitiated = false,
                        IsCreateCardSuccessful = false,
                        CreateProviderEndUserResponse = result.ToJson(),
                        ShouldPublish = customerCardDetailResponse.Status != RequestStatus.Inprogress,
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{nameof(CardRequestSagaConsumer)} :: {ex}");
                    await UpdateFailedCardRequest(cardRequest).ConfigureAwait(false);
                }
            }
        }

        private async Task UpdateFailedCardRequest(CardRequest cardRequest)
        {
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>();

                cardRequest.Status = CardRequestStatus.Failed;

                dbContext.CardRequests.Update(cardRequest);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task CreateCustomer(CardRequest cardRequest)
        {
            using (var scope = _provider.CreateScope())
            {
                try
                {
                    var country = await GetCountry(cardRequest, scope);

                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var result = await mediator.Send(new CreateCustomerCommand
                        {
                            Address = cardRequest.Address,
                            City = cardRequest.City,
                            Email = cardRequest.Email,
                            CountryIso = country.Id,
                            CustomerIdentity = cardRequest.CustomerId,
                            CustomerType = cardRequest.CustomerType,
                            FirstName = cardRequest.FirstName,
                            LastName = cardRequest.LastName,
                            MiddleName = cardRequest.MiddleName,
                            PhoneNumber = cardRequest.PhoneNumber,
                            DateOfBirth = cardRequest.DateOfBirth,
                            ProductId = cardRequest.ProductId,
                            PostalCode = cardRequest.PostalCode,
                        })
                        .ConfigureAwait(false);

                    await mediator.Send(new UpdateCardRequestProcessCommand
                    {
                        Status = CardRequestStatus.Inprogress,
                        CardRequestId = cardRequest.Id,
                        IsCreateCustomerInitiated = true,
                        IsCreateCustomerSuccessful = result.IsSuccess,
                        IsCreateCardInitiated = false,
                        IsCreateCardSuccessful = false,
                        IsCreateCardDetailsInitiated = false,
                        IsCreateCardDetailsSuccessful = false,
                        CreateCustomerResponse = result.ToJson(),
                        ShouldPublish = true,
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{nameof(CardRequestSagaConsumer)} :: {ex}");
                    await UpdateFailedCardRequest(cardRequest).ConfigureAwait(false);
                }
            }
        }

        private static async Task<Country> GetCountry(CardRequest cardRequest, IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>();
            var country = await dbContext.Countries.FirstOrDefaultAsync(x => x.Iso == cardRequest.CountryIso);
            return country;
        }

        private static async Task<Currency> GetCurrency(CardRequest cardRequest, IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>();
            var currency = await dbContext.Currencies.FirstOrDefaultAsync(x => x.Code == cardRequest.CurrencyCode);
            return currency;
        }
    }
}