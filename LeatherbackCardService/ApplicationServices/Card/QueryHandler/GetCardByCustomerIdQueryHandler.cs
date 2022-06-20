using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using ApplicationServices.Card.Query;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.BaseResponse;

namespace ApplicationServices.Card.QueryHandler
{
    public class
        GetCardByCustomerIdQueryHandler : IRequestHandler<GetCardByCustomerIdQuery,
            Result<List<CardViewModel>>>
    {
        private readonly ICardServiceDbContext _dbContext;

        public GetCardByCustomerIdQueryHandler(ICardServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<CardViewModel>>> Handle(GetCardByCustomerIdQuery request,
            CancellationToken cancellationToken)
        {
            var cardQueryable =
                _dbContext.Cards.Include(x => x.CardDetails)
                    .ThenInclude(x => x.ProviderEndUser)
                    .ThenInclude(x => x.Customer)
                    .Include(x => x.CardDetails)
                    .ThenInclude(x => x.Currency)
                    .OrderByDescending(x => x.Created)
                    .Where(x => x.CardDetails.ProviderEndUser.CustomerId == request.CustomerId);

            var getCardViewModels = await cardQueryable.Select(x => new CardViewModel
                {
                    Id = x.Id,
                    CardDesign = x.LeatherBackCardDesign,
                    CardProgramme = x.CardDetails.CardProgramme,
                    CardStatus = x.CardStatus,
                    CardType = x.CardDetails.CardType,
                    CardHolderName = x.CardHolderName,
                    CurrencyCode = x.CardDetails.Currency.Code,
                    CustomerId = x.CardDetails.ProviderEndUser.CustomerId,
                    RequestId = x.CardDetails.CardRequestId,
                    Email = x.CardDetails.ProviderEndUser.Customer.Email,
                    Status = x.CardStatus,
                    PhoneNumber = x.CardDetails.ProviderEndUser.Customer.PhoneNumber,
                    MaskedNumber = x.MaskedPan,
                    ExpiryDate = x.ExpiryDate,
                      AccountId = x.CardDetails.AccountId,
                })
                .ToListAsync(cancellationToken);

            return Result.Ok(getCardViewModels);
        }
    }
}