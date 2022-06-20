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
    public class ChangeCardPinCommandHandler : IRequestHandler<ChangeCardPinCommand, Result>
    {
        private readonly ILogger<ChangeCardPinCommandHandler> _logger;
        private readonly ICardServiceDbContext _dbContext;
        private readonly ICardPinService _cardPinService;
        private readonly IOtpService _otpService;

        public ChangeCardPinCommandHandler(ILogger<ChangeCardPinCommandHandler> logger, ICardServiceDbContext dbContext,
            ICardPinService cardPinService, IOtpService otpService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cardPinService = cardPinService;
            _otpService = otpService;
        }

        public async Task<Result> Handle(ChangeCardPinCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ChangeCardPinCommandHandler)} :: Request: {request.ToJson()}");

            var card = await _dbContext.Cards.Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.Customer)
                .Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.CardProvider)
                .FirstOrDefaultAsync(x => x.Id == request.CardId, cancellationToken: cancellationToken);

            if (card is null)
            {
                var result = Result.Fail(EnumUtility.GetEnumDescription(CardStatus.CardNotFound));

                _logger.LogInformation($"{nameof(ChangeCardPinCommandHandler)} :: {result.ToJson()}");
                return result;
            }

            var validateOtp = await _otpService.ValidateOtp(request.UserName, request.Otp, cancellationToken);
            if (validateOtp.IsSuccess == false) return Result.Fail("Invalid OTP");

            var updateCardPinResult = await _cardPinService.ResetCardPin(request.Pin, card, cancellationToken);

            return updateCardPinResult;
        }
    }
}