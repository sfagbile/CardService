using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.RailsBank.Ledger.Query;
using Domain.Entities.RailsBank;
using Domain.Exceptions;
using Domain.Interfaces;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Ledger.QueryHandler
{
    public class GetRailsBankLedgersByCustomerIdQueryHandler: IRequestHandler<GetRailsBankLedgersByCustomerIdQuery, Result<List<GetLedgerByCustomerIdViewModel>>>
    {
        private readonly IRailsBankLedgerRepository _railsBankLedgerRepository;
        public GetRailsBankLedgersByCustomerIdQueryHandler(IRailsBankLedgerRepository railsBankLedgerRepository)
        {
            _railsBankLedgerRepository = railsBankLedgerRepository;
        }
        
        public async Task<Result<List<GetLedgerByCustomerIdViewModel>>> Handle(GetRailsBankLedgersByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            var ledgersFromDb = await _railsBankLedgerRepository.Search(x => x.CustomerId == request.CustomerId);
            var railsBankLedgers = ledgersFromDb as RailsBankLedger[] ?? ledgersFromDb.ToArray();
            if (!railsBankLedgers.Any()) throw new EntityNotFoundException("Ledger not found for customer");
            var ledgers = railsBankLedgers.Select(x => new GetLedgerByCustomerIdViewModel() { CustomerId = x.CustomerId, EndUserId = x.EndUserId, LedgerId = x.LedgerId }).ToList();
            return Result.Ok(ledgers);
        }
    }
}