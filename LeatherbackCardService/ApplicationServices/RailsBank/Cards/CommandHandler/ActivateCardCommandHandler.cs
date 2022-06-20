using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.RailsBank.Cards.Command;
using Domain.Interfaces;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Cards.CommandHandler
{
    public class ActivateCardCommandHandler : IRequestHandler<ActivateCardCommand, Result<string>>
    {
        private readonly IRailsBankCardIssuanceRepository _railsBankCardIssuanceRepository;
        private readonly IRailsBankService _railsBankService;

        public ActivateCardCommandHandler(IRailsBankCardIssuanceRepository railsBankCardIssuanceRepository,
            IRailsBankService railsBankService)
        {
            _railsBankCardIssuanceRepository = railsBankCardIssuanceRepository;
            _railsBankService = railsBankService;
        }

        public async Task<Result<string>> Handle(ActivateCardCommand request, CancellationToken cancellationToken)
        {
            var activateCard = new ActivateRailsBankCardViewModel() {CardId = request.CardId};
            
            var result =
                await _railsBankService
                    .Post<RailsBankCardActivatedViewModel, RailsBankError, ActivateRailsBankCardViewModel>(activateCard,
                        $"customer/cards/{request.CardId}/activate");
            
            var (response, error, isSuccessful) = result.Value;
            
            if (isSuccessful)
            {
                var card = await _railsBankCardIssuanceRepository.GetFirstOrDefault(x =>
                    x.CardId == Guid.Parse(response.CardId));
                
                card.IsCardActivated = true;
                _railsBankCardIssuanceRepository.UpdateAsync(card);
                var isCardActivated = await _railsBankCardIssuanceRepository.Save();
                
                return isCardActivated
                    ? Result.Ok<string>(response.CardId)
                    : Result.Fail<string>(string.Join(",", error.Path.Select(n => n.ToString()).ToArray()));
            }

            var errorPath = error.Path != null
                ? string.Join(",", error.Path.Select(n => n.ToString()).ToArray())
                : error.Error;
            
            return Result.Fail<string>($"The following fields are not populated: {errorPath}");
        }
    }
}