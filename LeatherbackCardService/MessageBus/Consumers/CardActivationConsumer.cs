using System;
using System.Threading.Tasks;
using ApplicationServices.CardManagement.Commands;
using ApplicationServices.Interfaces;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.InternalBusMessages;

namespace MessageBus.Consumers
{
    public class CardActivationConsumer : IConsumer<CardActivationMessage>
    {
        private readonly ILogger<CardActivationConsumer> _logger;
        private readonly IServiceProvider _provider;

        public CardActivationConsumer(ILogger<CardActivationConsumer> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public async Task Consume(ConsumeContext<CardActivationMessage> context)
        {
            try
            {
                var cardActivationRequestMessage = context.Message;
                _logger.LogInformation($"CardActivationConsumer :: {cardActivationRequestMessage.ToJson()}");

                using (var scope = _provider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

                    var card = await scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>()
                        .Cards.Include(x => x.CardDetails)
                        .FirstOrDefaultAsync(x => x.Id == cardActivationRequestMessage.CardId);

                    if (card is null)
                    {
                        _logger.LogInformation(
                            $"CardActivationConsumer :: CardId not found - {cardActivationRequestMessage.CardId}");
                        return;
                    }

                    var result = await ProcessCardActivationRequest(cardActivationRequestMessage, mediator, publisher);
                    _logger.LogInformation($"CardActivationConsumer :: {result.ToJson()}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString(), "On Calling CardActivationConsumer");
            }
        }

        private static async Task<Result> ProcessCardActivationRequest(
            CardActivationMessage cardActivationMessage, IMediator mediator, IMessagePublisher publisher)
        {
            var result = await mediator.Send(new CardActivationCommand
            {
                CardId = cardActivationMessage.CardId,
            });

            var cardActivationResponseModel = result.Value;

            return Result.Ok(cardActivationResponseModel);
        }
    }
}