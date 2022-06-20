using System;
using ApplicationServices.Card.Model;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Command
{
    public class ApproveCardRequestCommand : IRequest<Result>
    {
        public Guid CardRequestId { get; set; }
    }
}