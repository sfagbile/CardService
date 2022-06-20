using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.WebHooks.RailsBank.Commands
{
    public class EndUserNotificationCommand : NotificationBase, IRequest<Result>
    {
        public string EndUserId { get; set; }
        public string EndUserStatus { get; set; }
    }
}