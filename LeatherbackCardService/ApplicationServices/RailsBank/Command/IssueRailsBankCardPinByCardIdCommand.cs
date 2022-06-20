using System;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Command
{
    public class IssueRailsBankCardPinByCardIdCommand: IRequest<Result>
    {
        public Guid CardId { get; set; }
    }
}