using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Common.Options;
using ApplicationServices.RailsBank.Ledger.Command;
using Domain.Entities.RailsBank;
using Domain.Interfaces;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Ledger.CommandHandler
{
    public class CreatePhysicalLedgerCommandHandler : IRequestHandler<CreatePhysicalLedgerCommand, Result<string>>
    {
        private readonly IRailsBankService _railsBankService;
        private readonly RailsBankRoot _railsBankRoot;
        private readonly IRailsBankLedgerRepository _railsBankLedgerRepository;

        public CreatePhysicalLedgerCommandHandler(IRailsBankService railsBankService,
            IOptionsMonitor<RailsBankRoot> railsBank, IRailsBankLedgerRepository railsBankLedgerRepository)
        {
            _railsBankService = railsBankService;
            _railsBankLedgerRepository = railsBankLedgerRepository;
            _railsBankRoot = railsBank.CurrentValue;
        }

        public async Task<Result<string>> Handle(CreatePhysicalLedgerCommand request,
            CancellationToken cancellationToken)
        {
            var ledgerViewModel =
                PhysicalLedgerRequestViewModel.Factory.CreateInstance(request.HolderId, _railsBankRoot,
                    request.CustomerId);
            ledgerViewModel.PartnerProduct = _railsBankRoot.PartnerProduct;

            var result =
                await _railsBankService
                    .Post<CreatedPhysicalLedgerResponseViewModel, RailsBankError, PhysicalLedgerRequestViewModel>(
                        ledgerViewModel, "customer/ledgers");
            var (response, error, isSuccessful) = result.Value;

            if (isSuccessful)
            {
                var ledger = RailsBankLedger.Factory.CreateInstance(request.CustomerId, Guid.Parse(request.HolderId),
                    Guid.Parse(response.LedgerId));

                await _railsBankLedgerRepository.InsertAsync(ledger.Value);
                var isLedgerMapped = await _railsBankLedgerRepository.Save();

                return isLedgerMapped
                    ? Result.Ok<string>(response.LedgerId)
                    : Result.Fail<string>(string.Join(",", error.Path.Select(n => n.ToString()).ToArray()));
            }

            var errorPath = error.Path != null
                ? string.Join(",", error.Path.Select(n => n.ToString()).ToArray())
                : error.Error;

            return Result.Fail<string>($"The following fields are not populated: {errorPath}");
        }
    }
}