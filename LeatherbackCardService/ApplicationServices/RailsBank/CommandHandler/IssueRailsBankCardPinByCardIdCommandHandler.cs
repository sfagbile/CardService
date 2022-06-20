using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Common.Options;
using ApplicationServices.RailsBank.Command;
using Domain.Entities.Customer;
using Domain.Entities.RailsBank;
using Domain.Interfaces;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.CommandHandler
{
    public class
        IssueRailsBankCardPinByCardIdCommandHandler : IRequestHandler<IssueRailsBankCardPinByCardIdCommand, Result>
    {
        private readonly IRailsBankService _railsBankService;
        private readonly RailsBankRoot _railsBankRoot;
        private readonly IRailsBankCardIssuanceRepository _railsBankCardIssuanceRepository;

        public IssueRailsBankCardPinByCardIdCommandHandler(IRailsBankService railsBankService,
            IOptionsMonitor<RailsBankRoot> railsBank, IRailsBankCardIssuanceRepository railsBankCardIssuanceRepository)
        {
            _railsBankService = railsBankService;
            _railsBankCardIssuanceRepository = railsBankCardIssuanceRepository;
            _railsBankRoot = railsBank.CurrentValue;
        }

        public async Task<Result> Handle(IssueRailsBankCardPinByCardIdCommand request,
            CancellationToken cancellationToken)
        {
            var result =
                await _railsBankService.Get<IssueRailsBankCardPinByCardIdResponseViewModel, RailsBankError>(
                    $"customer/cards/{request.CardId}/pin");
            var (response, error, isSuccessful) = result.Value;

            if (isSuccessful)
            {
                var cardFromDb =
                    await _railsBankCardIssuanceRepository.GetFirstOrDefault(x => x.CardId == request.CardId);

                var card = CustomerCardDetails.IssuePin(cardFromDb);
                _railsBankCardIssuanceRepository.UpdateAsync(card);

                var isCardSuspended = await _railsBankCardIssuanceRepository.Save();
                if (isCardSuspended)
                {
                    //call email service here
                    return Result.Ok();
                }

                return Result.Fail("Unable to issue pin");
            }

            var errorPath = error.Path != null
                ? string.Join(",", error.Path.Select(n => n.ToString()).ToArray())
                : error.Error;
            return Result.Fail<string>($"The following fields are not populated: {errorPath}");
        }
    }
}