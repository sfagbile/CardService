using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.CardManagement.Commands;
using ApplicationServices.Interfaces.Card;
using Domain.Entities.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.Extensions;
using Shared.Utility;

namespace ApplicationServices.CardManagement.CommandHandlers
{
    public class ValidateCardPinCommandHandler : IRequestHandler<ValidateCardPinCommand, Result>
    {
        private readonly ILogger<ValidateCardPinCommandHandler> _logger;
        private readonly ICardServiceDbContext _dbContext;
        private readonly ICardPinService _cardPinService;

        public ValidateCardPinCommandHandler(ILogger<ValidateCardPinCommandHandler> logger,
            ICardServiceDbContext dbContext, ICardPinService cardPinService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cardPinService = cardPinService;
        }

        public async Task<Result> Handle(ValidateCardPinCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ValidateCardPinCommandHandler)} :: Request: {request.ToJson()}");

            Result result;
            var card = await _dbContext.Cards.Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.Customer)
                .Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.CardProvider)
                .FirstOrDefaultAsync(x => x.Id == request.CardId, cancellationToken: cancellationToken);

            if (card is null)
            {
                result = Result.Fail(EnumUtility.GetEnumDescription(CardStatus.CardNotFound));

                _logger.LogInformation($"{nameof(ValidateCardPinCommandHandler)} :: {result.ToJson()}");
                return result;
            }

            var validateCardPinResult =
                await _cardPinService.ValidateCardPin(request.Pin, card, cancellationToken);

            if (validateCardPinResult.IsSuccess is not false) return Result.Ok();

            result = Result.Fail("Invalid Pin");
            _logger.LogInformation($"{nameof(ValidateCardPinCommandHandler)} :: {result.ToJson()}");

            return result;
        }
    }
}