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
        GetCardLimitTypeQueryHandler : IRequestHandler<GetCardLimitTypeQuery, Result<List<GetCardLimitTypeViewModel>>>
    {
        private readonly ICardServiceDbContext _dbContext;

        public GetCardLimitTypeQueryHandler(ICardServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<GetCardLimitTypeViewModel>>> Handle(GetCardLimitTypeQuery request,
            CancellationToken cancellationToken)
        {
            var cardLimitTypes = await _dbContext.CardLimitTypes.OrderByDescending(x => x.Created)
                .ToListAsync(cancellationToken);

            var cardLimitTypeViewModels = cardLimitTypes.Select(x => new GetCardLimitTypeViewModel
            {
                Description = x.Description,
                Type = x.Type,
                CardLimitTypeId = x.Id,
            }).ToList();

            return Result.Ok(cardLimitTypeViewModels);
        }
    }
}