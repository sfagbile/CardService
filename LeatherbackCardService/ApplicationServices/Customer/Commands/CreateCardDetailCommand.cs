using System;
using ApplicationServices.Customer.Model;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Customer.Commands
{
    public class CreateCardDetailCommand : IRequest<Result<CreateCardDetailResponseModel>>
    {
        public Guid CustomerId { get; set; }
        public Guid CardRequestId { get; set; }
        public CustomerType CustomerType { get; set; }
        public CardType CardType { get; set; }
        public Guid CurrencyId { get; set; }
        public CardDesignType LeatherBackCardDesign { get; set; }
    }
}