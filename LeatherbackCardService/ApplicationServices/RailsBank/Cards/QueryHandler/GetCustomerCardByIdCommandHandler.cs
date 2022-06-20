using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Common.Options;
using ApplicationServices.RailsBank.Cards.Query;
using Domain.Interfaces;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Cards.QueryHandler
{
    public class GetCustomerCardByIdCommandHandler: IRequestHandler<GetCustomerCardByIdCommand, Result<GetRailsBankCardByIdResponseViewModel>>
    {
        private readonly IRailsBankService _railsBankService;
        private readonly RailsBankRoot _railsBankRoot;
        private readonly IRailsBankCardIssuanceRepository _railsBankCardIssuanceRepository;
        
        public GetCustomerCardByIdCommandHandler(IRailsBankService railsBankService, IOptionsMonitor<RailsBankRoot> railsBank, IRailsBankCardIssuanceRepository railsBankCardIssuanceRepository)
        {
            _railsBankService = railsBankService;
            _railsBankCardIssuanceRepository = railsBankCardIssuanceRepository;
            _railsBankRoot = railsBank.CurrentValue;
        }
        
        public async Task<Result<GetRailsBankCardByIdResponseViewModel>> Handle(GetCustomerCardByIdCommand request, CancellationToken cancellationToken)
        {
            var result = await _railsBankService.Get<GetRailsBankCardByIdResponseViewModel, RailsBankError>($"customer/cards/{request.CardId}");
            var (response, error, isSuccessful) = result.Value;
            if (isSuccessful)
            {
                return Result.Ok(response);
            }
            var errorPath= error.Path != null ? string.Join(",",  error.Path.Select(n => n.ToString()).ToArray()) : error.Error;
            return Result.Fail<GetRailsBankCardByIdResponseViewModel>($"The following fields are not populated: {errorPath}");
        }
    }
}