using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Customer.Model;
using Domain.Entities.Cards;
using Shared.BaseResponse;

namespace ApplicationServices.Interfaces.CardDetailServices
{
    public interface ICardDetailService : IStrategyProcessor
    {
        bool HasWebHook { get; }

        Task<Result<CreateLedgerResponseModel>> CreatedVirtualCardDetails (CardDetail cardDetail,
            CancellationToken cancellationToken);
        
        Task<Result<CreateLedgerResponseModel>> CreatePhysicalCardDetails (CardDetail cardDetail,
            CancellationToken cancellationToken);
    }
}