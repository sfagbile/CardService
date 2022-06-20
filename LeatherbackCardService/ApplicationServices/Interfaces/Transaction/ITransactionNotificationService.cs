using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.CardIssuance.Model;
using ApplicationServices.TransactionNotification.Models;
using Domain.Entities.Customers;
using Shared.BaseResponse;

namespace ApplicationServices.Interfaces.Transaction
{
    public interface ITransactionNotificationService: IStrategyProcessor
    {
        bool HasWebHook { get; }
        
        Task<Result<TransactionNotificationResponseModel>> SendCreditNotification(TransactionNotificationRequestModel transactionNotificationRequestModel,
            CancellationToken cancellationToken);
        
        Task<Result<TransactionNotificationResponseModel>> SendDebitNotification(TransactionNotificationRequestModel transactionNotificationRequestModel,
            CancellationToken cancellationToken);
    }
}