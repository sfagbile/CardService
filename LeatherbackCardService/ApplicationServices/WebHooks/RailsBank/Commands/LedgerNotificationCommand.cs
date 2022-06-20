using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.WebHooks.RailsBank.Commands
{
    public class LedgerNotificationCommand: NotificationBase, IRequest<Result>
    {
        public string LedgerId { get; set; }
    }
}