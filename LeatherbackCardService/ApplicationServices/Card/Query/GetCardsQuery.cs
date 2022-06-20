using System;
using ApplicationServices.Card.Model;
using Domain.Entities.Enums;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Query
{
    public class GetCardsQuery : PaginationParameter,
        IRequest<Result<PagedResponse<CardViewModel>>>
    {
        public Guid? CustomerId { get; set; }
        public Guid? CardRequestId { get; set; }
        public CardType? CardType { get; set; }
        public string CardHolderName { get; set; }
        public CustomerType? CustomerType { get; set; }
        public CardStatus? Status { get; set; }
    }
}