using System;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.TransactionNotification.Commands
{
    public class DebitTransactionNotificationCommand : IRequest<Result>
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public Guid CardId { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string Remarks { get; set; }
    }
}