using System;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.CardManagement.Commands
{
    public class ChangeCardPinCommand : IRequest<Result>
    {
        public Guid CardId { get; set; }
        public string Pin { get; set; }
        public string UserName { get; set; }
        public string Otp { get; set; }
    }
}