using System;
using System.Collections.Generic;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Ledger.Query
{
    public class GetRailsBankLedgersByCustomerIdQuery: IRequest<Result<List<GetLedgerByCustomerIdViewModel>>>
    {
        public Guid CustomerId { get; set; }
    }
}