using System;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Cards.Command
{
    public class IssueVirtualCardCommand: IRequest<Result<string>>
    {
        public Guid LedgerId { get; set; }
        public Guid CustomerId { get; set; }
    }
}