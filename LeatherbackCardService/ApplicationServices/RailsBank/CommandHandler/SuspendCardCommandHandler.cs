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
    public class SuspendCardCommandHandler : IRequestHandler<SuspendCardCommand, Result>
    {
        private readonly IRailsBankService _railsBankService;
        private readonly RailsBankRoot _railsBankRoot;
        private readonly IRailsBankCardIssuanceRepository _railsBankCardIssuanceRepository;

        public SuspendCardCommandHandler(IRailsBankService railsBankService, IOptionsMonitor<RailsBankRoot> railsBank,
            IRailsBankCardIssuanceRepository railsBankCardIssuanceRepository)
        {
            _railsBankService = railsBankService;
            _railsBankCardIssuanceRepository = railsBankCardIssuanceRepository;
            _railsBankRoot = railsBank.CurrentValue;
        }

        public async Task<Result> Handle(SuspendCardCommand request, CancellationToken cancellationToken)
        {
            var cardToBeSuspended = new SuspendRailsBankCardViewModel() {SuspendReason = request.Reason};

            var result =
                await _railsBankService
                    .Post<SuspendRailsBankCardResponseViewModel, RailsBankError, SuspendRailsBankCardViewModel>(
                        cardToBeSuspended, $"customer/cards/{request.CardId}/suspend");

            var (response, error, isSuccessful) = result.Value;
            if (isSuccessful)
            {
                var cardFromDb =
                    await _railsBankCardIssuanceRepository.GetFirstOrDefault(x => x.CardId == request.CardId);

                var card = CustomerCardDetails.SuspendCard(cardFromDb, request.Reason);
                _railsBankCardIssuanceRepository.UpdateAsync(card);

                var isCardSuspended = await _railsBankCardIssuanceRepository.Save();
                if (isCardSuspended)
                {
                    return Result.Ok();
                }

                return Result.Fail("Unable to suspend card");
            }

            var errorPath = error.Path != null
                ? string.Join(",", error.Path.Select(n => n.ToString()).ToArray())
                : error.Error;
            
            return Result.Fail<string>($"The following fields are not populated: {errorPath}");
        }
    }
}