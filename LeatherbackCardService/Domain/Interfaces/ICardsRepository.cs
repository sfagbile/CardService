using Domain.Entities.Cards;
using Shared.Repository;

namespace Domain.Interfaces
{
    public interface ICardsRepository : ICommandRepository<Card>, IQueryRepository<Card>
    {
    }
}