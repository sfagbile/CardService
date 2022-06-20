using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Interfaces.Transaction;
using ApplicationServices.TransactionNotification.Models;
using ApplicationServices.TransactionNotification.Models.RailsBank;
using ApplicationServices.ViewModels.RailsBank;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using Shared.BaseResponse;

namespace ApplicationServices.TransactionNotification.Processors
{
    public class RailsBankTransactionNotificationProcessor : ITransactionNotificationService
    {
        private readonly IRailsBankService _railsBankService;

        public RailsBankTransactionNotificationProcessor(IRailsBankService railsBankService)
        {
            _railsBankService = railsBankService;
        }

        public string ResolverValue { get; } = nameof(Provider.RailsBank);
        public bool HasWebHook { get; } = false;

        public async Task<Result<TransactionNotificationResponseModel>> SendCreditNotification(
            TransactionNotificationRequestModel transactionNotificationRequestModel,
            CancellationToken cancellationToken)
        {
            var result =
                await _railsBankService
                    .Post<RailsBankCreditVirtualLedgerResponseModel, RailsBankError, RailsBankCreditVirtualLedgerRequestModel>(
                        new RailsBankCreditVirtualLedgerRequestModel
                        {
                            Amount = transactionNotificationRequestModel.Amount.ToString(CultureInfo.InvariantCulture),
                            LedgerId = transactionNotificationRequestModel.Card.CardDetails.ProviderLedgerId,
                            TransactionMeta = new RailsBankTransactionMeta
                            {
                                TransactionId = transactionNotificationRequestModel.TransactionId.ToString(),
                                TransactionDate = transactionNotificationRequestModel.TransactionDateTime
                            }
                        },
                        "customer/transactions/manual-credit");

            if (result.IsSuccess)
                return Result.Ok(new TransactionNotificationResponseModel
                {
                    Status = TransactionStatus.Successful,
                    ProviderResponse = result.Value.ToJson(),
                    TransactionId = transactionNotificationRequestModel.TransactionId
                });

            return Result.Fail<TransactionNotificationResponseModel>(
                new TransactionNotificationResponseModel
                {
                    Status = TransactionStatus.Failed,
                    ProviderResponse = result.Value.ToJson(),
                    TransactionId = transactionNotificationRequestModel.TransactionId
                }, "Failed", "");
        }

        public async Task<Result<TransactionNotificationResponseModel>> SendDebitNotification(
            TransactionNotificationRequestModel transactionNotificationRequestModel,
            CancellationToken cancellationToken)
        {
            var result =
                await _railsBankService
                    .Post<RailsBankDebitVirtualLedgerResponseModel, RailsBankError, RailsBankDebitVirtualLedgerRequestModel>(
                        new RailsBankDebitVirtualLedgerRequestModel
                        {
                            Amount = transactionNotificationRequestModel.Amount.ToString(CultureInfo.InvariantCulture),
                            LedgerId = transactionNotificationRequestModel.Card.CardDetails.ProviderLedgerId,
                            TransactionMeta = new RailsBankTransactionMeta
                            {
                                TransactionId = transactionNotificationRequestModel.TransactionId.ToString(),
                                TransactionDate = transactionNotificationRequestModel.TransactionDateTime
                            }
                        },
                        "customer/transactions/manual-debit");

            if (result.IsSuccess)
                return Result.Ok(new TransactionNotificationResponseModel
                {
                    Status = TransactionStatus.Successful,
                    ProviderResponse = result.Value.ToJson(),
                    TransactionId = transactionNotificationRequestModel.TransactionId
                });

            return Result.Fail<TransactionNotificationResponseModel>(new TransactionNotificationResponseModel
            {
                Status = TransactionStatus.Failed,
                ProviderResponse = result.Value.ToJson(),
                TransactionId = transactionNotificationRequestModel.TransactionId
            }, "Failed", "");
        }
    }
}