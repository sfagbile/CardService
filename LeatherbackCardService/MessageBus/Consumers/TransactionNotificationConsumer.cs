using System;
using System.Threading.Tasks;
using ApplicationServices.TransactionNotification.Commands;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using LeatherbackSharedLibrary.Enums;
using LeatherbackSharedLibrary.Messages.Card;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;

namespace MessageBus.Consumers
{
    public class TransactionNotificationConsumer : IConsumer<TransactionNotificationMessage>
    {
        private readonly ILogger<TransactionNotificationConsumer> _logger;
        private readonly IServiceProvider _provider;

        public TransactionNotificationConsumer(ILogger<TransactionNotificationConsumer> logger,
            IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public async Task Consume(ConsumeContext<TransactionNotificationMessage> context)
        {
            try
            {
                var transactionNotificationMessage = context.Message;
                _logger.LogInformation($"TransactionNotificationConsumer :: {transactionNotificationMessage.ToJson()}");

                using (var scope = _provider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var card = await scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>()
                        .Cards.Include(x => x.CardDetails)
                        .FirstOrDefaultAsync(x => x.Id == transactionNotificationMessage.CardId);

                    if (card is null)
                    {
                        _logger.LogInformation(
                            $"TransactionNotificationConsumer :: CardId not found : {transactionNotificationMessage.CardId}");
                        return;
                    }

                    var result = await ProcessTransactionNotification(transactionNotificationMessage, mediator);
                    _logger.LogInformation($"TransactionNotificationConsumer :: {result.ToJson()}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString(), "On Calling TransactionNotificationConsumer");
            }
        }

        private static async Task<Result> ProcessTransactionNotification(
            TransactionNotificationMessage transactionNotificationMessage,
            IMediator mediator)
        {
            if (transactionNotificationMessage.TransactionNotificationType ==
                CardTransactionNotificationType.Credit)
                return await mediator.Send(new CreditTransactionNotificationCommand
                {
                    Amount = transactionNotificationMessage.Amount,
                    Remarks = transactionNotificationMessage.Remark,
                    CardId = transactionNotificationMessage.CardId,
                    TransactionDateTime = DateTime.Now,
                    TransactionId = transactionNotificationMessage.TransactionId
                });

            return await mediator.Send(new DebitTransactionNotificationCommand
            {
                Amount = transactionNotificationMessage.Amount,
                Remarks = transactionNotificationMessage.Remark,
                CardId = transactionNotificationMessage.CardId,
                TransactionDateTime = DateTime.Now,
                TransactionId = transactionNotificationMessage.TransactionId
            });
        }
    }
}