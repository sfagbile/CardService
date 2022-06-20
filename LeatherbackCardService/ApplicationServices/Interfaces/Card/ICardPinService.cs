using System;
using System.Threading;
using System.Threading.Tasks;
using Shared.BaseResponse;

namespace ApplicationServices.Interfaces.Card
{
    public interface ICardPinService
    {
        Task<Result<string>> SaveCardPin(string pin, Domain.Entities.Cards.Card card,
            CancellationToken cancellationToken);

        Task<Result<string>> ValidateCardPin(string pin, Domain.Entities.Cards.Card card,
            CancellationToken cancellationToken);

        Task<Result<string>> UpdateCardPin(string currentPin, string newPin, Domain.Entities.Cards.Card card,
            CancellationToken cancellationToken);

        Task<Result<string>> ResetCardPin(string currentPin, Domain.Entities.Cards.Card card,
            CancellationToken cancellationToken);
    }
}