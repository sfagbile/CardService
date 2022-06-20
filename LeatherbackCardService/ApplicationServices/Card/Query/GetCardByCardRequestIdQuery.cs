using System;
using System.Collections.Generic;
using ApplicationServices.Card.Model;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Query
{
    public class GetCardByCardRequestIdQuery : IRequest<Result<GetCardRequestResponseViewModel>>
    {
        public Guid CardRequestId { get; set; }
    }
}