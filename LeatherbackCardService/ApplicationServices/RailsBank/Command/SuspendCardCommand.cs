using System;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Command
{
    public class SuspendCardCommand: IRequest<Result>
    {
        public string Reason { get; set; }
        public Guid CardId { get; set; }
    }
}