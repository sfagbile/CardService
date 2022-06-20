using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using ApplicationServices.Card.Query;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.BaseResponse;

namespace ApplicationServices.Card.QueryHandler
{
    public class GetCardDetailsQueryHandler : IRequestHandler<GetCardDetailsQuery, Result<GetCardDetailsViewModel>>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCardDetailsQueryHandler(ICardServiceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<GetCardDetailsViewModel>> Handle(GetCardDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var card = await _dbContext.Cards.Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.Customer)
                .Include(x => x.CardDetails)
                .ThenInclude(x => x.Currency)
                .FirstOrDefaultAsync(x => x.Id == request.CardId, cancellationToken: cancellationToken);

            if (card is null) return Result.Fail<GetCardDetailsViewModel>("No card is found that matches your search");

            var cardDetailsViewModel = new GetCardDetailsViewModel
            {
                Customer = card.CardDetails.ProviderEndUser.Customer,
                Id = card.Id,
                CardDesign = card.LeatherBackCardDesign,
                CardLimits = _dbContext.CardLimits.Include(x => x.CardLimitTypes)
                    .Where(x => x.CardId == card.Id)
                    .Select(x => new CardLimitViewModel
                    {
                        Amount = x.MaxAmount,
                        CardLimitType = x.CardLimitTypes.Type,
                        CardLimitTypeId = x.CardLimitTypeId,
                    })
                    .ToList(),
                CardProgramme = card.CardDetails.CardProgramme,
                CardStatus = card.CardStatus,
                CardType = card.CardDetails.CardType,
                CurrencyCode = card.CardDetails.Currency.Code,
                CustomerId = card.CardDetails.ProviderEndUser.CustomerId,
                RequestId = card.CardDetails.CardRequestId,
                CardHolderName = card.CardHolderName,
                MaskedPan = card.MaskedPan,
                ExpiryDate = card.ExpiryDate,
                AccountId = card.CardDetails.AccountId,
            };

            return Result.Ok(cardDetailsViewModel);
        }
    }
}