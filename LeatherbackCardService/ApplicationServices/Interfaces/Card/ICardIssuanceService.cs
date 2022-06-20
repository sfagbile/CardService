using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.CardIssuance.Model;
using Domain.Entities.Cards;
using Domain.Entities.Customers;
using Shared.BaseResponse;

namespace ApplicationServices.Interfaces.Card
{
    public interface ICardIssuanceService : IStrategyProcessor
    {
        bool HasWebHook { get; }

        Task<Result<IssueVirtualCardRespondModel>> CreateVirtualCard(CardDetail customerCardDetail,
            CancellationToken cancellationToken);

        Task<Result<IssueVirtualCardRespondModel>> CreatePhysicalCard(CardDetail customerCardDetail,
            CancellationToken cancellationToken);
    }
}