using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.CardManagement.Models;
using Shared.BaseResponse;

namespace ApplicationServices.Interfaces.Card
{
    public interface ICardManagementService : IStrategyProcessor
    {
        Task<Result<CardSuspensionResponseModel>> SuspendCard(CardSuspensionRequestModel cardClosureRequestModel,
            CancellationToken cancellationToken);

        Task<Result<CardClosureResponseModel>> CloseCard(CardClosureRequestModel cardClosureRequestModel,
            CancellationToken cancellationToken);
        
        Task<Result<CardActivationResponseModel>> ActivateCard(CardActivationRequestModel cardActivationRequestModel,
            CancellationToken cancellationToken);
        
        bool HasWebHook { get; }
    }
}