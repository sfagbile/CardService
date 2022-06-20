using System;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Ledger.Command
{
    public class CreateVirtualLedgerCommand: IRequest<Result<string>>
    {
        public Guid HolderId { get; set; }
        public Guid CustomerId { get; set; }
        public string CountryIso3 { get; set; }
    }
}