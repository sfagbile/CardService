using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.TransactionNotification.Models.RailsBank
{
    public class RailsBankDebitVirtualLedgerRequestModel
    {
        public RailsBankTransactionMeta TransactionMeta { get; set; }
        public string Amount { get; set; }
        public string LedgerId { get; set; }
    }
}