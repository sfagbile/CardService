using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Common.Options;
using ApplicationServices.RailsBank.Cards.Command;
using AutoMapper;
using Domain.Entities.Customer;
using Domain.Entities.RailsBank;
using Domain.Exceptions;
using Domain.Interfaces;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Cards.CommandHandler
{
    public class IssueVirtualCardCommandHandler : IRequestHandler<IssueVirtualCardCommand, Result<string>>
    {
        private readonly IRailsBankService _railsBankService;
        private readonly IMapper _mapper;
        private readonly RailsBankRoot _providers;
        private readonly IRailsBankCardIssuanceRepository _railsBankCardIssuanceRepository;
        private readonly ICustomerRepository _railsBankEndUserRepository;
        private readonly IRailsBankLedgerRepository _railsBankLedgerRepository;

        public IssueVirtualCardCommandHandler(IRailsBankService railsBankService,
            IOptionsMonitor<RailsBankRoot> provider, IMapper mapper,
            IRailsBankCardIssuanceRepository railsBankCardIssuanceRepository,
            ICustomerRepository railsBankEndUserRepository,
            IRailsBankLedgerRepository railsBankLedgerRepository)
        {
            _railsBankService = railsBankService;
            _mapper = mapper;
            _railsBankCardIssuanceRepository = railsBankCardIssuanceRepository;
            _railsBankEndUserRepository = railsBankEndUserRepository;
            _railsBankLedgerRepository = railsBankLedgerRepository;
            _providers = provider.CurrentValue;
        }

        public async Task<Result<string>> Handle(IssueVirtualCardCommand request, CancellationToken cancellationToken)
        {
            var ledger = VirtualCardRequestViewModel.Factory.CreateInstance(request.LedgerId.ToString(), _providers);
            if (ledger == null) throw new EntityNotFoundException("Ledger not found");
            
            var serviceResponse =
                await _railsBankService.Post<VirtualCardResponseViewModel, RailsBankError, VirtualCardRequestViewModel>(
                    ledger, "customer/cards");
            
            var (result, error, isSuccessful) = serviceResponse.Value;
            
            if (isSuccessful)
            {
                var endUser =
                    await _railsBankEndUserRepository.GetFirstOrDefault(x => x.CustomerId == request.CustomerId);
                var endUserCard = CustomerCardDetails.Factory.CreateInstance(request.CustomerId,
                    Guid.Parse(result.CardId), request.LedgerId);
                
                await _railsBankCardIssuanceRepository.InsertAsync(endUserCard.Value);
                var isCardMappedToLedger = await _railsBankCardIssuanceRepository.Save();
                
                return isCardMappedToLedger
                    ? Result.Ok<string>(result.CardId)
                    : Result.Fail<string>(string.Join(",", error.Path.Select(n => n.ToString()).ToArray()));
            }

            var errorPath = error.Path != null
                ? string.Join(",", error.Path.Select(n => n.ToString()).ToArray())
                : error.Error;
            
            return Result.Fail<string>($"The following fields are not populated: {errorPath}");
        }
    }
}