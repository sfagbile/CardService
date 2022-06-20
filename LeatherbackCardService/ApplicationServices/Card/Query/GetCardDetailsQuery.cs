using System;
using ApplicationServices.Card.Model;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Query
{
    public class GetCardDetailsQuery: IRequest<Result<GetCardDetailsViewModel>>
    {
        public Guid CardId { get; set; }
    }
}