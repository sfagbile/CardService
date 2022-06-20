using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Customer.Model;
using Domain.Entities.Cards;
using Shared.BaseResponse;

namespace ApplicationServices.Interfaces.CardDetailServices
{
    public interface IProviderEndUserService : IStrategyProcessor
    {
        bool HasWebHook { get; }

        Task<Result<CreateProviderEndUserResponseModel>> CreateIndividualEndUser(Domain.Entities.Customers.Customer customer,
            CancellationToken cancellationToken);
        
        Task<Result<CreateProviderEndUserResponseModel>> CreateCompanyEndUser(Domain.Entities.Customers.Customer customer,
            CancellationToken cancellationToken);
    }
}