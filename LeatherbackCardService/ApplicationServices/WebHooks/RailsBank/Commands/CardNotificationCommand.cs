using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.WebHooks.RailsBank.Commands
{
    public class CardNotificationCommand : NotificationBase,  IRequest<Result>
    {
        public string CardId { get; set; }
    }
}