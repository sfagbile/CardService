using System.Collections.Generic;
using ApplicationServices.Card.Model;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Query
{
    public class GetCardLimitTypeQuery : IRequest<Result<List<GetCardLimitTypeViewModel>>>
    {
    }
}