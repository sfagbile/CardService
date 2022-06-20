using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using ApplicationServices.Card.Query;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.Card;
using AutoMapper;
using Domain.Entities.Cards.Specifications;
using Domain.Entities.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.BaseResponse;
using Shared.Encryption;
using Shared.Utility;

namespace ApplicationServices.Card.QueryHandler
{
    public class GetCardQueryHandler : IRequestHandler<GetCardQuery, Result<GetCardViewModel>>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStrategyResolver<ICardService> _cardStrategyResolver;
        private readonly ICardPinService _cardPinService;

        public GetCardQueryHandler(IMapper mapper, ICardServiceDbContext dbContext,
            IStrategyResolver<ICardService> cardStrategyResolver, ICardPinService cardPinService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _cardStrategyResolver = cardStrategyResolver;
            _cardPinService = cardPinService;
        }

        public async Task<Result<GetCardViewModel>> Handle(GetCardQuery request, CancellationToken cancellationToken)
        {
            var card = await _dbContext.Cards
                .Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.CardProvider)
                .FirstOrDefaultAsync(x => x.Id == request.CardId && x.CardStatus == CardStatus.CardActivated,
                    cancellationToken: cancellationToken);

            if (card is null)
                return Result.Fail<GetCardViewModel>("No card is found that matches your search");
            
            var validateCardPinResult =
                await _cardPinService.ValidateCardPin(request.Pin, card, cancellationToken);

            if (validateCardPinResult.IsSuccess is false)
                return Result.Fail<GetCardViewModel>("Invalid Pin");

            var cardViewModel = _mapper.Map<Domain.Entities.Cards.Card, GetCardViewModel>(card);

            var cardService = _cardStrategyResolver.GetService(card.CardDetails.ProviderEndUser.CardProvider.Name);

            if (cardService is null) return Result.Ok(cardViewModel);

            var cardDetails = await cardService.GetCardDetails(cardViewModel, cancellationToken);
            return cardDetails;
        }
    }
}