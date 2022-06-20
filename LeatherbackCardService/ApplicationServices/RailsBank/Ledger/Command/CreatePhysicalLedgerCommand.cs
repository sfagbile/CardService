using System;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Ledger.Command
{
    public class CreatePhysicalLedgerCommand: IRequest<Result<string>>
    {
        public string HolderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}