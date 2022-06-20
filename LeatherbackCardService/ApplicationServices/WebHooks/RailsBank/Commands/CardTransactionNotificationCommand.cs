using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.WebHooks.RailsBank.Commands
{
    public class CardTransactionNotificationCommand: NotificationBase, IRequest<Result>
    {
        public string TransactionId { get; set; }
        public string LedgerId {get; set; }
    }
}