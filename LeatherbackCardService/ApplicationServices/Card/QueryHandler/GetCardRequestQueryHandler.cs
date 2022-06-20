using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using ApplicationServices.Card.Query;
using Domain.Entities.Cards;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;

namespace ApplicationServices.Card.QueryHandler
{
    public class GetCardRequestQueryHandler : IRequestHandler<GetCardRequestQuery,
        Result<PagedResponse<GetCardRequestResponseViewModel>>>
    {
        private readonly ICardServiceDbContext _dbContext;
        private readonly ILogger<GetCardRequestQueryHandler> _logger;

        public GetCardRequestQueryHandler(ICardServiceDbContext dbContext, ILogger<GetCardRequestQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<PagedResponse<GetCardRequestResponseViewModel>>> Handle(GetCardRequestQuery request,
            CancellationToken cancellationToken)
        {
            var queryableCardRequest = _dbContext.CardRequests.OrderByDescending(x => x.Created)
                .AsQueryable();

            if (request.CardRequestId.HasValue)
                queryableCardRequest = queryableCardRequest.Where(x => x.Id == request.CardRequestId);

            if (request.CustomerId.HasValue)
                queryableCardRequest = queryableCardRequest.Where(x => x.CustomerId == request.CustomerId);

            if (request.CustomerType.HasValue)
                queryableCardRequest = queryableCardRequest.Where(x => x.CustomerType == request.CustomerType);

            if (request.CardType.HasValue)
                queryableCardRequest = queryableCardRequest.Where(x => x.CardType == request.CardType);

            if (request.Status.HasValue)
                queryableCardRequest = queryableCardRequest.Where(x => x.Status == request.Status);

            if (!string.IsNullOrWhiteSpace(request.CardHolderName))
                queryableCardRequest = queryableCardRequest.Where(x =>
                    x.CardDeliveryName.Contains(request.CardHolderName));

            var cardRequestResponseViewModels = queryableCardRequest
                .Select(a => GetCardRequestResponse(a, _dbContext))
                .AsQueryable();

            var pageSize = request.DisablePageLimit ? int.MaxValue : request.PageSize;

            var pagedRecord = await PagedResponse<GetCardRequestResponseViewModel>.Create(
                cardRequestResponseViewModels,
                request.Page, pageSize);

            return Result.Ok(pagedRecord);
        }

        private static GetCardRequestResponseViewModel GetCardRequestResponse(CardRequest cardRequest,
            ICardServiceDbContext dbContext)
        {
            var currency = dbContext.Currencies
                .FirstOrDefault(x => x.Code == cardRequest.CurrencyCode);

            var country = dbContext.Countries
                .FirstOrDefault(x => x.Iso == cardRequest.CountryIso);

            return new GetCardRequestResponseViewModel
            {
                Id = cardRequest.Id,
                Address = cardRequest.Address,
                City = cardRequest.City,
                Email = cardRequest.Email,
                Status = cardRequest.Status,
                AccountId = cardRequest.AccountId,
                CardType = cardRequest.CardType,
                CountryIso = cardRequest.CountryIso,
                CurrencyCode = cardRequest.CurrencyCode,
                CustomerId = cardRequest.CustomerId,
                CustomerType = cardRequest.CustomerType,
                FirstName = cardRequest.FirstName,
                LastName = cardRequest.LastName,
                MiddleName = cardRequest.MiddleName,
                PhoneNumber = cardRequest.PhoneNumber,
                PostalCode = cardRequest.PostalCode,
                DateOfBirth = cardRequest.DateOfBirth,
                Design = cardRequest.Design,
                CardDeliveryName = cardRequest.CardDeliveryName,
                CountryName = country?.Name,
                CurrencyName = currency?.Name,
                CreatedAt = cardRequest.Created,
                UpdatedAt = cardRequest.LastModified,
                CardLimits = string.IsNullOrWhiteSpace(cardRequest.TransactionLimit)
                    ? new List<CardLimitViewModel>()
                    : Newtonsoft.Json.JsonConvert.DeserializeObject<List<CardLimitViewModel>>(
                        cardRequest.TransactionLimit)
            };
        }
    }
}