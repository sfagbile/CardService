using System;
using System.Collections.Generic;
using ApplicationServices.Card.Model;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Query
{
    public class GetCardByCustomerIdQuery : IRequest<Result<List<CardViewModel>>>
    {
        public Guid CustomerId { get; set; }
    }
}