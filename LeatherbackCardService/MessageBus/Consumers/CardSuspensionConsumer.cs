using System;
using System.Threading.Tasks;
using ApplicationServices.CardManagement.Commands;
using ApplicationServices.Interfaces;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using LeatherbackSharedLibrary.Messages.Card;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.InternalBusMessages;

namespace MessageBus.Consumers
{
    public class CardSuspensionConsumer : IConsumer<CardSuspensionRequestMessage>
    {
        private readonly ILogger<CardSuspensionConsumer> _logger;
        private readonly IServiceProvider _provider;

        public CardSuspensionConsumer(ILogger<CardSuspensionConsumer> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public async Task Consume(ConsumeContext<CardSuspensionRequestMessage> context)
        {
            try
            {
                var cardSuspensionRequestMessage = context.Message;
                _logger.LogInformation($"CardSuspensionConsumer :: {cardSuspensionRequestMessage.ToJson()}");

                using (var scope = _provider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();
                    var card = await scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>()
                        .Cards.Include(x => x.CardDetails)
                        .FirstOrDefaultAsync(x => x.Id == cardSuspensionRequestMessage.CardId);

                    if (card is null)
                    {
                        _logger.LogInformation(
                            $"CardSuspensionConsumer :: CardId not found - {cardSuspensionRequestMessage.CardId}");
                        return;
                    }

                    var result = await ProcessCardSuspensionRequest(cardSuspensionRequestMessage, mediator, publisher);
                    _logger.LogInformation($"CardSuspensionConsumer :: {result.ToJson()}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString(), "On Calling CardSuspensionConsumer");
            }
        }

        private static async Task<Result> ProcessCardSuspensionRequest(
            CardSuspensionRequestMessage message, IMediator mediator, IMessagePublisher publisher)
        {
            var result = await mediator.Send(new CardSuspensionCommand
            {
                CardId = message.CardId,
                Reason = message.Reason,
            });

            var cardSuspensionResponseModel = result.Value;

            if (cardSuspensionResponseModel.Status != RequestStatus.Inprogress)
            {
                await publisher.Publish(new CardSuspensionMessage
                {
                    CardId = cardSuspensionResponseModel.CardId,
                    CardSuspensionStatus = cardSuspensionResponseModel.CardSuspensionStatus,
                });
            }

            return Result.Ok(cardSuspensionResponseModel);
        }
    }
}