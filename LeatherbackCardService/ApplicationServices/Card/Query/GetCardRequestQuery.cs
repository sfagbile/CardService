using System;
using ApplicationServices.Card.Model;
using Domain.Entities.Enums;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Query
{
    public class GetCardRequestQuery : PaginationParameter,
        IRequest<Result<PagedResponse<GetCardRequestResponseViewModel>>>
    {
        public Guid? CustomerId { get; set; }
        public Guid? CardRequestId { get; set; }
        public CardType? CardType { get; set; }
        
        public string CardHolderName { get; set; }
        public CustomerType? CustomerType { get; set; }
        public CardRequestStatus? Status { get; set; }
    }
}