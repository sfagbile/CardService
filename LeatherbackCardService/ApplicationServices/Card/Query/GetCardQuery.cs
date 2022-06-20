using System;
using ApplicationServices.Card.Model;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Query
{
    public class GetCardQuery: IRequest<Result<GetCardViewModel>>
    {
        public Guid CardId { get; set; }
        public string Pin { get; set; }
    }
}