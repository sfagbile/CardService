using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using Shared.BaseResponse;

namespace ApplicationServices.Interfaces.Card
{
    public interface ICardService : IStrategyProcessor
    {
        bool HasWebHook { get; }
        
        Task<Result<GetCardViewModel>> GetCardDetails(GetCardViewModel cardViewModel, CancellationToken cancellationToken);
    }
}