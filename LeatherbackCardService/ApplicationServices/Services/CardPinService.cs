using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Interfaces.Card;
using Domain.Entities.Cards;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.BaseResponse;
using Shared.Encryption;
using Shared.Utility;

namespace ApplicationServices.Services
{
    public class CardPinService : ICardPinService
    {
        private readonly ICardServiceDbContext _dbContext;

        public CardPinService(ICardServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<string>> SaveCardPin(string pin, Domain.Entities.Cards.Card card, CancellationToken cancellationToken)
        {
            
            if (card is null) return Result.Fail<string>($"Invalid card Id: {card.Id}");

            var cardPin = await _dbContext.CardPins
                .FirstOrDefaultAsync(x => x.CardId == card.Id, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (cardPin is not null)
            {
                var encryptedPin = EncryptionUtil.GenerateSha512Hash(card.Id + pin + cardPin.Id);
                _dbContext.CardPins.Update(cardPin);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return Result.Ok<string>($"Successful");
            }
           
            cardPin = CardPin.CreateInstance(card.Id, pin);
            card.IsPinIssued = true;
            
            await _dbContext.CardPins.AddAsync(cardPin, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Result.Ok<string>($"Successful");
        }

        public async Task<Result<string>> ValidateCardPin(string pin, Domain.Entities.Cards.Card card, CancellationToken cancellationToken)
        {
            if (card is null) return Result.Fail<string>($"Invalid card Id: {card.Id}");

            var cardPin =
                await _dbContext.CardPins.FirstOrDefaultAsync(x => x.CardId == card.Id,
                    cancellationToken: cancellationToken);

            if (cardPin is null) return Result.Fail<string>($"Invalid Pin");

            var encryptedPin = EncryptionUtil.GenerateSha512Hash(card.Id + pin + cardPin.Id);

            return !cardPin.Pin.Equals(encryptedPin)
                ? Result.Fail<string>($"Invalid Pin")
                : Result.Ok<string>($"Successful");
        }

        public async Task<Result<string>> UpdateCardPin(string currentPin, string newPin, Domain.Entities.Cards.Card card,
            CancellationToken cancellationToken)
        {

            var result = await ValidateCardPin(currentPin, card, cancellationToken);
            if (!result.IsSuccess) return result;
            
            var cardPin =
                await _dbContext.CardPins.FirstOrDefaultAsync(x => x.CardId == card.Id,
                    cancellationToken: cancellationToken);
            
            var encryptedPin = EncryptionUtil.GenerateSha512Hash(card.Id + newPin + cardPin.Id);
            cardPin.Pin = encryptedPin;
                
            _dbContext.CardPins.Update(cardPin);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Result.Ok<string>($"Successful");
        }

        public async Task<Result<string>> ResetCardPin(string pin, Domain.Entities.Cards.Card card, CancellationToken cancellationToken)
        {
            var cardPin =
                await _dbContext.CardPins.FirstOrDefaultAsync(x => x.CardId == card.Id,
                    cancellationToken: cancellationToken);
            
            var encryptedPin = EncryptionUtil.GenerateSha512Hash(card.Id + pin + cardPin.Id);
            cardPin.Pin = encryptedPin;
                
            _dbContext.CardPins.Update(cardPin);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Result.Ok<string>($"Successful");
        }
    }
}