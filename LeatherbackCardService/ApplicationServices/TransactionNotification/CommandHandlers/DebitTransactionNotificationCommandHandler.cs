using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.Transaction;
using ApplicationServices.TransactionNotification.Commands;
using ApplicationServices.TransactionNotification.Models;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.BaseResponse;

namespace ApplicationServices.TransactionNotification.CommandHandlers
{
    public class
        DebitTransactionNotificationCommandHandler : IRequestHandler<DebitTransactionNotificationCommand, Result>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IStrategyResolver<ITransactionNotificationService> _transactionNotificationStrategyProcessor;

        public DebitTransactionNotificationCommandHandler(ICardServiceDbContext dbContext,
            IStrategyResolver<ITransactionNotificationService> transactionNotificationStrategyProcessor)
        {
            _dbContext = dbContext;
            _transactionNotificationStrategyProcessor = transactionNotificationStrategyProcessor;
        }

        public async Task<Result> Handle(DebitTransactionNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var card = await _dbContext.Cards.Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.CardProvider)
                .FirstOrDefaultAsync(x => x.Id == request.CardId, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            var customerCardDetail = card.CardDetails;
            var cardProvider = customerCardDetail.ProviderEndUser.CardProvider;

            var transactionNotificationStrategyProcessor =
                _transactionNotificationStrategyProcessor.GetService(cardProvider.Name);

            var sendCreditNotificationResult = await transactionNotificationStrategyProcessor.SendDebitNotification(
                new TransactionNotificationRequestModel
                {
                    Amount = request.Amount,
                    Remarks = request.Remarks,
                    Card = card,
                    TransactionId = request.TransactionId,
                    TransactionDateTime = request.TransactionDateTime
                }, cancellationToken);

            return sendCreditNotificationResult;
        }
    }
}