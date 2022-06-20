using System;
using System.Threading.Tasks;
using ApplicationServices.Card.Command;
using ApplicationServices.Interfaces;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.InternalBusMessages;

namespace MessageBus.Consumers
{
    public class CreateCardRequestConsumer : IConsumer<CreateCardRequestMessage>
    {
        private readonly ILogger<CreateCardRequestConsumer> _logger;
        private readonly IServiceProvider _provider;

        public CreateCardRequestConsumer(ILogger<CreateCardRequestConsumer> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public async Task Consume(ConsumeContext<CreateCardRequestMessage> context)
        {
            try
            {
                var message = context.Message;
                using (var scope = _provider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var cardRequest = await dbContext.CardRequests
                        .FirstOrDefaultAsync(x => x.Id == message.Id)
                        .ConfigureAwait(false);

                    if (cardRequest is not null)
                    {
                        _logger.LogInformation(
                            $"CardRequestConsumer: CardRequest not found - {message.ToJson()}");
                        return;
                    }

                    var sex = GetSex(message);
                    if (sex == default)
                    {
                        _logger.LogInformation($"CardRequestConsumer: Invalid sex - {message.Sex}");
                        return;
                    }

                    var customerType = GetCustomerType(message);
                    if (customerType == default)
                    {
                        _logger.LogInformation(
                            $"CardRequestConsumer: Invalid customerType - {message.CustomerType}");
                        return;
                    }

                    var cardType = message.CardType.ToLower() switch
                    {
                        "physical" => CardType.Physical,
                        "virtual" => CardType.Virtual
                    };

                    if (cardType == default)
                    {
                        _logger.LogInformation(
                            $"CardRequestConsumer: Invalid cardType - {message.CardType}");
                        return;
                    }

                    var result = await mediator.Send(new CreateCardRequestCommand
                    {
                        Address = message.Address,
                        City = message.City,
                        Email = message.Email,
                        AccountId = message.AccountId,
                        CardType = cardType,
                        CountryIso = message.CountryIso,
                        CurrencyCode = message.CurrencyCode,
                        CustomerId = message.CustomerId,
                        CustomerType = customerType,
                        FirstName = message.FirstName,
                        LastName = message.LastName,
                        MiddleName = message.MiddleName,
                        PhoneNumber = message.PhoneNumber,
                        PostalCode = message.PostalCode,
                        DateOfBirth = message.DateOfBirth
                    }).ConfigureAwait(false);
                    
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString(), "On Calling CardRequestConsumer");
            }
        }

        private static CustomerType GetCustomerType(CreateCardRequestMessage message)
        {
            var customerType = message.CustomerType.ToLower() switch
            {
                "company" => CustomerType.Company,
                "individual" => CustomerType.Individual,
            };
            return customerType;
        }

        private static Sex GetSex(CreateCardRequestMessage message)
        {
            var sex = message.Sex.ToLower() switch
            {
                "male" => Sex.Male,
                "female" => Sex.Female,
            };
            return sex;
        }
    }
}