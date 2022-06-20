using System;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Command
{
    public class RejectCardRequestCommand : IRequest<Result>
    {
        public Guid CardRequestId { get; set; } 
        public string Reason { get; set; } 
    }
}