using System;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Cards.Query
{
    public class GetCustomerCardByIdCommand: IRequest<Result<GetRailsBankCardByIdResponseViewModel>>
    {
        public Guid CardId { get; set; }
    }
}