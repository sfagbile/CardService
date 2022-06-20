using System;
using ApplicationServices.CardIssuance.Model;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.CardIssuance.Commands
{
    public class IssueVirtualCardCommand : IRequest<Result<VirtualCardRespondModel>>
    {
        public Guid CardRequestId { get; set; }
    }
}