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
    public class CardClosureConsumer : IConsumer<CardClosureRequestMessage>
    {
        private readonly ILogger<CardClosureConsumer> _logger;
        private readonly IServiceProvider _provider;

        public CardClosureConsumer(ILogger<CardClosureConsumer> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }


        public async Task Consume(ConsumeContext<CardClosureRequestMessage> context)
        {
            try
            {
                var cardClosureRequestMessage = context.Message;
                _logger.LogInformation($"CardClosureConsumer :: {cardClosureRequestMessage.ToJson()}");

                using (var scope = _provider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();
                    var card = await scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>()
                        .Cards.Include(x => x.CardDetails)
                        .FirstOrDefaultAsync(x => x.Id == cardClosureRequestMessage.CardId);

                    if (card is null)
                    {
                        _logger.LogInformation(
                            $"CardClosureConsumer :: CardId not found - {cardClosureRequestMessage.CardId}");
                        return;
                    }

                    var result = await ProcessCardClosureRequest(cardClosureRequestMessage, mediator, publisher);
                    _logger.LogInformation($"CardClosureConsumer :: {result.ToJson()}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString(), "On Calling CardClosureConsumer");
            }
        }

        private static async Task<Result> ProcessCardClosureRequest(
            CardClosureRequestMessage message, IMediator mediator, IMessagePublisher publisher)
        {
            var result = await mediator.Send(new CardClosureCommand
            {
                CardId = message.CardId,
                // Reason = message.Reason
            });

            var cardClosureRequestModel = result.Value;

           if (cardClosureRequestModel.Status != RequestStatus.Inprogress)
            {
                await publisher.Publish(new CardClosureMessage
                {
                    CardId = cardClosureRequestModel.CardId,
                    CardClosureStatus = cardClosureRequestModel.CardClosureStatus,
                });
            }

            return Result.Ok(cardClosureRequestModel);
        }
    }
}