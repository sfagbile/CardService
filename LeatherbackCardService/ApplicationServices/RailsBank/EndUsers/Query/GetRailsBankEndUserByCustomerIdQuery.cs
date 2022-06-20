using System;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.EndUsers.Query
{
    public class GetRailsBankEndUserByCustomerIdQuery: IRequest<Result<GetEndUserViewModel>>
    {
        public Guid CustomerId { get; set; }
    }
}