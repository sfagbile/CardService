using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using ApplicationServices.Card.Query;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;

namespace ApplicationServices.Card.QueryHandler
{
    public class GetCardsQueryHandler : IRequestHandler<GetCardsQuery,
        Result<PagedResponse<CardViewModel>>>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly ILogger<GetCardsQueryHandler> _logger;

        public GetCardsQueryHandler(ICardServiceDbContext dbContext, ILogger<GetCardsQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<PagedResponse<CardViewModel>>> Handle(GetCardsQuery request,
            CancellationToken cancellationToken)
        {
            var queryableCards = _dbContext.Cards
                .Include(x => x.CardDetails)
                .ThenInclude(x => x.ProviderEndUser)
                .ThenInclude(x => x.Customer)
                .Include(x => x.CardDetails)
                .ThenInclude(x => x.Currency)
                .OrderByDescending(x => x.Created)
                .AsQueryable();

            if (request.CardRequestId.HasValue)
                queryableCards = queryableCards.Where(x => x.Id == request.CardRequestId);

            if (request.CustomerId.HasValue)
                queryableCards =
                    queryableCards.Where(x => x.CardDetails.ProviderEndUser.CustomerId == request.CustomerId);

            if (request.CustomerType.HasValue)
                queryableCards = queryableCards.Where(x =>
                    x.CardDetails.ProviderEndUser.Customer.CustomerType == request.CustomerType);

            if (request.CardType.HasValue)
                queryableCards = queryableCards.Where(x => x.CardDetails.CardType == request.CardType);

            if (request.Status.HasValue)
                queryableCards = queryableCards.Where(x => x.CardStatus == request.Status);

            if (!string.IsNullOrWhiteSpace(request.CardHolderName))
                queryableCards = queryableCards.Where(x => 
                    x.CardHolderName.Contains(request.CardHolderName));

            var responseViewModels = queryableCards.Select(a => GetCardResponse(a, _dbContext))
                .AsQueryable();

            var pageSize = request.DisablePageLimit ? int.MaxValue : request.PageSize;

            var pagedRecord = await PagedResponse<CardViewModel>.Create(responseViewModels, request.Page, pageSize);

            return Result.Ok(pagedRecord);
        }

        private static CardViewModel GetCardResponse(Domain.Entities.Cards.Card card, ICardServiceDbContext dbContext)
        {
            return new CardViewModel
            {
                Id = card.Id,
                CardDesign = card.LeatherBackCardDesign,
                CardProgramme = card.CardDetails?.CardProgramme,
                CardStatus = card.CardStatus,
                CardType = card.CardDetails.CardType,
                CardHolderName = card.CardHolderName,
                CurrencyCode = card.CardDetails.Currency.Code,
                CustomerId = card.CardDetails.ProviderEndUser.CustomerId,
                RequestId = card.CardDetails.CardRequestId,
                Email = card.CardDetails.ProviderEndUser?.Customer?.Email,
                Status = card.CardStatus,
                PhoneNumber = card.CardDetails.ProviderEndUser?.Customer?.PhoneNumber,
                CardLimits = dbContext.CardLimits.Include(x => x.CardLimitTypes)
                    .Where(x => x.CardId == card.Id)
                    .Select(x => new CardLimitViewModel
                    {
                        Amount = x.MaxAmount,
                        CardLimitType = x.CardLimitTypes.Type,
                        CardLimitTypeId = x.CardLimitTypeId,
                    })
                    .ToList(),
                MaskedNumber = card.MaskedPan,
                ExpiryDate = card.ExpiryDate,
                CreatedAt = card.Created,
                UpdatedAt = card.LastModified
            };
        }
    }
}