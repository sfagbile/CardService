using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.RailsBank.EndUsers.Query;
using Domain.Exceptions;
using Domain.Interfaces;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.EndUsers.QueryHandler
{
    public class
        GetRailsBankEndUserByCustomerIdQueryHandler : IRequestHandler<GetRailsBankEndUserByCustomerIdQuery,
            Result<GetEndUserViewModel>>
    {
        private readonly ICustomerRepository _railsBankEndUserRepository;

        public GetRailsBankEndUserByCustomerIdQueryHandler(ICustomerRepository railsBankEndUserRepository)
        {
            _railsBankEndUserRepository = railsBankEndUserRepository;
        }

        public async Task<Result<GetEndUserViewModel>> Handle(GetRailsBankEndUserByCustomerIdQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _railsBankEndUserRepository.GetFirstOrDefault(x => x.CustomerId == request.CustomerId);

            if (result == null) throw new EntityNotFoundException("End user not found for customer");

            return Result.Ok(new GetEndUserViewModel() {EndUserId = result.EndUserId, CustomerId = result.CustomerId});
        }
    }
}