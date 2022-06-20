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
        GetCardByCardRequestIdQueryHandler : IRequestHandler<GetCardByCardRequestIdQuery,
            Result<GetCardRequestResponseViewModel>>
    {
        private readonly ICardServiceDbContext _dbContext;

        public GetCardByCardRequestIdQueryHandler(ICardServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetCardRequestResponseViewModel>> Handle(GetCardByCardRequestIdQuery request,
            CancellationToken cancellationToken)
        {
            var cardRequest = await
                _dbContext.CardRequests.FirstOrDefaultAsync(x => x.Id == request.CardRequestId,
                    cancellationToken: cancellationToken);

            if (cardRequest is null)
                return Result.Fail<GetCardRequestResponseViewModel>(
                    $"card request id {request.CardRequestId} not found");

            var currency = await _dbContext.Currencies
                .FirstOrDefaultAsync(x => x.Code == cardRequest.CurrencyCode, cancellationToken: cancellationToken)
                .ConfigureAwait(false);


            var country = await _dbContext.Countries
                .FirstOrDefaultAsync(x => x.Iso3 == cardRequest.CountryIso, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            var cardViewModel = new GetCardRequestResponseViewModel
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
                CurrencyName = currency?.Name,
                CountryName = country?.Name,
            };

            return Result.Ok(cardViewModel);
        }
    }
}